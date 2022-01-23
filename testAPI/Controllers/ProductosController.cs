using APIsystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testAPI.Data;

namespace testAPI.Controllers
{
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AplicationDBContext _bd;
        public ProductosController(AplicationDBContext db)
        {
            _bd = db;
        }

        [HttpGet("[controller]/getAll")]
        public async Task<IActionResult> GetProduct()
        {
            List<Productos> lista = await _bd.Productos.Where(p => !p.isDeleted).OrderBy(p => p.ID).ToListAsync();
            return Ok(lista);
        }
        [HttpPost("[controller]/new")]
        public async Task<IActionResult> Create([FromBody] Productos value)
        {
            await _bd.Productos.AddAsync(value);
            await _bd.SaveChangesAsync();
            logActivity(value, value, TypeOperation.addStack);
            return Ok(value.ID);
        }

        [HttpPost("[controller]/update")]
        public async Task<IActionResult> Update([FromBody] Productos value)
        {
            var product = _bd.Productos.Find(value.ID);
            if (product == null)
            {
                return BadRequest();
            }
            logActivity(product, value, TypeOperation.editStack);
            product.Descripcion = value.Descripcion;
            product.Precio = value.Precio;
            product.Existencia = value.Existencia;

            _bd.SaveChanges();
            return Ok(value.ID);
        }

        [HttpPost("[controller]/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _bd.Productos.Find(id);
            if (product == null)
            {
                return BadRequest();
            }
            logActivity(product, product, TypeOperation.deleteStack);
            product.isDeleted = true;

            _bd.SaveChanges();
            return Ok(id);
        }

        [HttpPost("[controller]/updateAllPrice")]
        public async Task<IActionResult> UpdateAllPrice(decimal price)
        {
            List<Productos> lista = await _bd.Productos.Where(p => !p.isDeleted).ToListAsync();
            lista.ForEach(a => a.Precio = price);
            logActivity(lista[0], lista[0], TypeOperation.allPriceUpdate);
            _bd.SaveChanges();
            return Ok(lista);
        }

        [HttpPost("[controller]/Audit")]
        public async Task<IActionResult> GetAudit()
        {
            List<Actividades> lista = await _bd.Actividades.OrderBy(a => a.Id).ToListAsync();
            return Ok(lista);
        }

        private void logActivity(Productos before, Productos after, TypeOperation Operation)
        {
            switch (Operation)
            {
                case TypeOperation.addStack:
                    setAudit(before.ID, "Descripcion", before.Descripcion, before.Descripcion, Operation);
                    setAudit(before.ID, "Precio", before.Precio.ToString(), before.Precio.ToString(), Operation);
                    setAudit(before.ID, "Existencia", before.Existencia.ToString(), before.Existencia.ToString(), Operation);

                    break;
                case TypeOperation.editStack:
                    if (before.Descripcion != after.Descripcion)
                    {
                        setAudit(before.ID, "Descripcion", before.Descripcion, after.Descripcion, Operation);
                    }
                    if (before.Precio != after.Precio)
                    {
                        setAudit(before.ID, "Precio", before.Precio.ToString(), after.Precio.ToString(), Operation);
                    }
                    if (before.Existencia != after.Existencia)
                    {
                        setAudit(before.ID, "Existencia", before.Existencia.ToString(), after.Existencia.ToString(), Operation);
                    }

                    break;
                case TypeOperation.deleteStack:
                    setAudit(before.ID, "isDeleted", before.isDeleted.ToString(), (true).ToString(), Operation);
                    break;
                case TypeOperation.allPriceUpdate:
                    setAudit(0, "Precio", before.Precio.ToString(), after.Precio.ToString(), Operation);
                    break;
            }
        }
        private void setAudit(int PrductoID, string field, string before, string after, TypeOperation Operation)
        {
            Actividades act = new Actividades();
            act.PrductoID = PrductoID;
            act.Date = DateTime.Now;
            act.Operation = Operation;
            act.field = field;
            act.Before = before;
            act.After = after;

            _bd.Actividades.Add(act);
            _bd.SaveChanges();
        }
    }
}
