using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiRest.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        [HttpPost]
        [Route("autenticar")]
        public IHttpActionResult Autenticar(Models.Login login)
        {

            var obj = new Models.USUARIOS();

            using (var db = new Models.dbHornitoEntities())
            {
                obj = db.USUARIOS.Where(p => p.USERNAME.Equals(login.Usuario) &&
                                        p.CLAVE.Equals(login.Clave)).FirstOrDefault();
            }

            if (obj != null)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Usuario);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }

        }



    }
}
