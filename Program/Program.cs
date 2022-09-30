using BaltnetaSmsApiCore;

var service = new SmsService("", "");
 var RESP=  await service.SendSmsAsync("phone","message","sender");


Console.ReadLine( );