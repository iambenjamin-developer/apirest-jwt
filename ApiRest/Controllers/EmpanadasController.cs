using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ApiRest.Models;

namespace ApiRest.Controllers
{
    [Authorize]
    public class EmpanadasController : ApiController
    {
        // GET: api/Empanadas
        public IEnumerable<EMPANADAS> Get()
        {
            IEnumerable<EMPANADAS> lista = new List<EMPANADAS>();

            using (var db = new dbHornitoEntities())
            {

                lista = db.EMPANADAS.ToList();
            }

            return lista;
        }

        // GET: api/Empanadas/5
        public EMPANADAS Get(int id)
        {
            var obj = new EMPANADAS();

            using (var db = new dbHornitoEntities())
            {
                obj = db.EMPANADAS.Where(p => p.ID.Equals(id)).FirstOrDefault();
            }

            return obj;
        }

        // POST: api/Empanadas
        public async Task<string> Post(EMPANADAS obj)
        {
            using (var db = new dbHornitoEntities())
            {
                db.EMPANADAS.Add(obj);
                db.SaveChanges();
            }

            // Create a task to execute CountCharacters() function
            // CountCharacters() function returns int, so we created Task<int>
            Task<bool> tarea = new Task<bool>(EnviarConfirmacionPorMail);
            tarea.Start();

            bool mailEnviado = await tarea;

            if (mailEnviado == true)
            {
                return "Se envio correctamente el mail de confirmacion!";
            }
            else
            {

                return "Error en el envio de mails!";
            }

        }

        // PUT: api/Empanadas/5
        public void Put(EMPANADAS parametro)
        {
        }

        // DELETE: api/Empanadas/5
        public void Delete(int id)
        {
        }



        private bool EnviarConfirmacionPorMail()
        {

            bool mailEnviado = false;

            string displayName = "Hornito Santiagueño";
            string user = "catercapro@gmail.com";
            string pass = "69696969696";

            var client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            // Include credentials if the server requires them.
            client.Credentials = new System.Net.NetworkCredential(user, pass);

            //direccion del remitente
            var from = new System.Net.Mail.MailAddress(user, displayName, System.Text.Encoding.UTF8);
            //////////////////////////////////////////////////////////////////////////////////////

            var message = new System.Net.Mail.MailMessage();
            //remitente (usuario desde dónde se envía)
            message.From = from;

            //destinatario principal
            var to = new System.Net.Mail.MailAddress("benjacordoba@gmail.com", "Benjamon", System.Text.Encoding.UTF8);
            //destinatarios
            message.To.Add(to);

            // Asunto del mensaje
            message.Subject = "Nuevo sabor";

            //cuerpo del mensaje --> que lo tome de una variable cuerpoMensaje
            message.Body = @"Se agrego un nuevo sabor a la lista!!";

            // Add a carbon copy recipient.
            var copy = new System.Net.Mail.MailAddress("catercapro@gmail.com");
            message.CC.Add(copy);


            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            try
            {
                client.Send(message);
                Thread.Sleep(5000);
                //se setea en true si el mail se envia correctamente
                mailEnviado = true;

            }
            catch (Exception ex)
            {
                // TODO: agregar log
                mailEnviado = false;
            }

            return mailEnviado;
        }


    }

}

