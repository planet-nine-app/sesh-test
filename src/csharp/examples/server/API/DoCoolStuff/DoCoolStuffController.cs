using Microsoft.AspNetCore.Mvc;
using System;
using SessionlessExample.Server.Models;
using System.Text.Json;
using SessionlessNET.Impl;
using SessionlessNET.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SessionlessExample.Server.Controllers;


    [ApiController]
    [Route("/cool-stuff")]
    public class DoCoolStuffController: ControllerBase
    {
        Vault Vault;
        Sessionless Sessionless;
        const string privPath = "./priv.key";
        const string pubPath = "./pub.key";
        static void VaultSaver(KeyPairHex pair) {
            
        }
        static KeyPairHex? VaultGetter() {
            
            return new("", "");
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult PostJson(Models.DoCoolStuffModel input) {
            Vault = new Vault(VaultGetter, VaultSaver);
            Sessionless = new(Vault);

            UserModel user = DemoPersistenceController.getUser(input.uuid);

            if(user is null) {
                return Problem("No user found");
            }
                        
            var postedSignature = Request.Headers["Signature"];

            if(postedSignature is null) {
                return Problem("No signature");
            }

            MessageSignatureHex signature = new(postedSignature);

            var message = JsonSerializer.Serialize<DoCoolStuffModel>(input);

            SignedMessage signedMessage = new SessionlessNET.Models.SignedMessage(message, signature);

            if(!Sessionless.VerifySignature(signedMessage, user.pubKey)) {
                return Problem("Auth error");
            }

            var resp = new Models.CoolStuffModel("Double Cool!");

            return Ok(resp);
        }
    }

