using Azure.Core.Serialization;
using CryptoWallet.API.Data;
using CryptoWallet.API.Models;
using CryptoWallet.API.Models.DTOs;
using CryptoWallet.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoWallet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        public TransactionsController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpGet("cryptos")]
        public IActionResult GetCrypto()
        {
            var cryptos = _context.cryptos.ToList();
            return Ok(cryptos);
        }
        //COMPRA Y VENTA DE CRIPTOMONEDA
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
        {
            try
            {

                var crypto = _context.cryptos.FirstOrDefault(c => c.Code == transactionDto.CryptoCode);
                var action = _context.transactionActions.FirstOrDefault(a => a.Name == transactionDto.Action);

                if (action == null || crypto == null || transactionDto.CryptoAmount <= 0)
                {
                    return BadRequest("Los datos no pueden ser nulos ni la cantidad menor a 0");
                }
                //Validacion de saldo para ventas
                if (transactionDto.Action == "sale")
                {
                    var totalComprado = _context.transactions.
                        Where(t => t.CryptoId == crypto.Id && t.TransactionAction.Name == "purchase").
                        Sum(t => t.CryptoAmount);
                    var totalVendido = _context.transactions.
                        Where(t => t.CryptoId == crypto.Id && t.TransactionAction.Name == "sale").
                        Sum(t => t.CryptoAmount);

                    var saldoDisponible = totalComprado - totalVendido;

                    if (transactionDto.CryptoAmount > saldoDisponible)
                    {
                        return BadRequest($"No tienes suficiente {crypto.Code}. Saldo disponible: {saldoDisponible}");
                    }
                }

                //Consultamos a CryptoYa
                string url = $"https://criptoya.com/api/satoshitango/{transactionDto.CryptoCode}/ars";
                string response = await _httpClient.GetStringAsync(url);
               

                //Deserializar la respuesta de CryptoYa. Convierto un string JSON en un objeto C# para poder usar sus valores.
                var cryptoYaData = JsonSerializer.Deserialize<CryptoYaResponse>(response);

                //Calcular el monto segun si es compra o venta
                decimal precio;
                if (transactionDto.Action == "purchase")
                {
                    precio = (decimal)cryptoYaData.TotalAsk; //si es compra, uso TotalAsk
                }
                else
                {
                    precio = (decimal)cryptoYaData.TotalBid; //si es venta, uso TotalBid
                }

                decimal money = transactionDto.CryptoAmount * precio;


                //Crear y guardar la transacción en la base de datos.

                var transaction = new CryptoTransaction
                {
                    CryptoId = crypto.Id,
                    TransactionActionId = action.Id,
                    CryptoAmount = transactionDto.CryptoAmount,
                    Money = money,
                    DateTime = transactionDto.DateTime
                };
                _context.transactions.Add(transaction);
                _context.SaveChanges();

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: "+ ex.Message);
            }
        }

        //HISTORIAL DE TRANSACCIONES
        [HttpGet]
        public IActionResult GetTransaction()
        {
            if (!_context.transactions.Any())
            {
                return NotFound("No se encontraron transacciones.");
            }

            var historial = _context.transactions.Include(t => t.TransactionAction).Include(t => t.Crypto).OrderBy(t => t.DateTime).Select(t => new
            {
                Id = t.Id,
                CryptoCode = t.Crypto.Code,
                Action = t.TransactionAction.Name,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                DateTime = t.DateTime
            }).ToList();
            return Ok(historial);
        }

        //DETALLE DE TRANSACCION
        [HttpGet("{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = _context.transactions.Include(t => t.TransactionAction).Include(t => t.Crypto).FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            var transactionDto = new
            {
                Id = transaction.Id,
                CryptoCode = transaction.Crypto.Code,
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                Action = transaction.TransactionAction.Name,
                DateTime = transaction.DateTime
            };
            return Ok(transactionDto);
        }
        //ACTUALIZACION DE TRANSACCION
        [HttpPatch("{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] TransactionDto transactionDto)
        {
            var crypto = _context.cryptos.FirstOrDefault(c => c.Code == transactionDto.CryptoCode);
            var action = _context.transactionActions.FirstOrDefault(a => a.Name == transactionDto.Action);
            if (crypto == null || action == null)
            {
                return BadRequest("Datos inválidos.");
            }

            var transaction = _context.transactions.Include(t => t.Crypto).Include(t => t.TransactionAction).FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.CryptoId = crypto.Id;
            transaction.TransactionActionId = action.Id;
            transaction.CryptoAmount = transactionDto.CryptoAmount;
            transaction.DateTime = transactionDto.DateTime;
            transaction.Money = transactionDto.Money;

            _context.transactions.Update(transaction);
            _context.SaveChanges();

            return Ok(transaction);
        }

        //ELIMINACION DE TRANSACCION
        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _context.transactions.FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound("Transaccion no pudo borrarse.");
            }
            _context.transactions.Remove(transaction);
            _context.SaveChanges();
            return Ok(new { estado = "Ok" });
        }

        //ESTADO ACTUAL DE LA CARTERA
        [HttpGet("estado")]
        public async Task<IActionResult> EstadoActual()
        {
            try
            {
                if (!_context.transactions.Any())   
                {
                    return NotFound("No hay transacciones registradas.");
                }

                var estado = _context.transactions.Include(t => t.Crypto).Include(t => t.TransactionAction).GroupBy(t => t.CryptoId).ToList();
                var lista = new List<dynamic>(); //lista dinamica para almacenar objetos anonimos con cryptoCode y cantidad y poder usarlo en el otro foreach.
                foreach (var item in estado)
                {
                    var totalComprado = item.Where(t => t.TransactionAction.Name == "purchase").Sum(t => t.CryptoAmount);
                    var totalVendido = item.Where(t => t.TransactionAction.Name == "sale").Sum(t => t.CryptoAmount);
                    var cantidad = totalComprado - totalVendido;

                    //Filtro por los que tienen saldo
                    if (cantidad > 0)
                    {
                        lista.Add(new
                        {
                            CryptoCode = item.First().Crypto.Code, //tomo la primera para acceder al objeto Crypto y obtener su código
                            Cantidad = cantidad
                        });
                    }
                }


                //Consultamos a CryptoYa el valor actual para calcular el valor total en pesos.

                var resultados = new List<object>(); //Lista para almacenar los resultados individuales de cada crypto,
                                                     //recorrerlos mediante el foreach y luego mostrarlos
                decimal totalPesos = 0;

                //Recorro cada crypto para consultar su valor actual(uso foreach porque cada crypto es una llamada distina a la API)
                foreach (var item in lista)
                {
                    string url = $"https://criptoya.com/api/satoshitango/{item.CryptoCode}/ars";
                    string response = await _httpClient.GetStringAsync(url);

                    //Deserializar la respuesta de CryptoYa
                    var cryptoYaData = JsonSerializer.Deserialize<CryptoYaResponse>(response);

                    decimal valorPesos = item.Cantidad * (decimal)cryptoYaData.TotalBid;
                    totalPesos += valorPesos;

                    resultados.Add(new
                    {
                        CryptoCode = item.CryptoCode,
                        Cantidad = item.Cantidad,
                        ValorPesos = valorPesos
                    });
                }
                return Ok(new { cryptos = resultados, totalPesos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }
    }
}
