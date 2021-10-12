using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MojangStatus.Controllers
{
	public class StatusController : Controller
	{
		[HttpGet]
		[Route("/check")]
		public ActionResult Get()
		{
			PingReply mainReply = new Ping().Send("minecraft.net", 150);
			PingReply sessionReply = new Ping().Send("session.minecraft.net", 150);
			PingReply accountReply = new Ping().Send("account.mojang.com", 150);
			PingReply authReply = new Ping().Send("authserver.mojang.com", 150);
			PingReply oldsessionReply = new Ping().Send("sessionserver.mojang.com", 150);
			PingReply apiReply = new Ping().Send("api.mojang.com", 150);
			PingReply texturesReply = new Ping().Send("textures.minecraft.net", 150);
			PingReply oldmainReply = new Ping().Send("mojang.com", 150);

			StringBuilder builder = new();
			builder.Append($"[{{\"minecraft.net\": \"{ConvertReply(mainReply)}\"}},");
			builder.Append($"{{\"session.minecraft.net\": \"{ConvertReply(sessionReply)}\"}},");
			builder.Append($"{{\"account.mojang.com\": \"{ConvertReply(accountReply)}\"}},");
			builder.Append($"{{\"authserver.mojang.com\": \"{ConvertReply(authReply)}\"}},");
			builder.Append($"{{\"sessionserver.mojang.com\": \"{ConvertReply(oldsessionReply)}\"}},");
			builder.Append($"{{\"api.mojang.com\": \"{ConvertReply(apiReply)}\"}},");
			builder.Append($"{{\"textures.minecraft.net\": \"{ConvertReply(texturesReply)}\"}},");
			builder.Append($"{{\"mojang.com\": \"{ConvertReply(oldmainReply)}\"}}]");

			return this.Ok(builder.ToString());
		}

		private String ConvertReply(PingReply reply)
		{
			return reply.Status switch
			{
				IPStatus.Success => "green",
				IPStatus.BadRoute => "yellow",
				IPStatus.PacketTooBig => "yellow",
				_ => "red"
			};
		}
	}
}
