using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Helpers.Common
{
    public class Plantillas_Correo
    {
        public static string TitleRecoverPassword = "Recuperar contraseña - Momentos Don Fernando";

        public string Mail_ChangePassword(string Password)
        {
            // mensaje del correo
            StringBuilder strbMensaje_p = new StringBuilder(); //constructor de cadena cuerpo de correo
            strbMensaje_p.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>"); //agrega encabezado
            strbMensaje_p.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            strbMensaje_p.AppendLine("<head>");
            strbMensaje_p.AppendLine("<meta http-equiv='X-UA-Compatible' content='IE=edge' />");
            strbMensaje_p.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            strbMensaje_p.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1' />");
            strbMensaje_p.AppendLine("<title>Notificación</title>");
            strbMensaje_p.AppendLine("<style type='text/css'>");
            strbMensaje_p.AppendLine("body {margin: 0; padding: 0; -webkit-text-size-adjust: none; -ms-text-size-adjust: none; background: #ffffff;}");
            strbMensaje_p.AppendLine("html {width: 100%;}");
            strbMensaje_p.AppendLine("table { border-spacing: 0; border-collapse: collapse;}");
            strbMensaje_p.AppendLine("table td { border-collapse: collapse; }");
            strbMensaje_p.AppendLine("@media only screen and (max-width:640px) {");
            strbMensaje_p.AppendLine("body { width: auto !important;}");
            strbMensaje_p.AppendLine("table [class=main] { width: 85% !important;}");
            strbMensaje_p.AppendLine("table [class=full] { width: 100% !important; margin: 0px auto;}");
            strbMensaje_p.AppendLine("table [class=two-left-inner] { width: 90% !important; margin: 0px auto; }}");
            strbMensaje_p.AppendLine("@media only screen and (max-width:479px) {");
            strbMensaje_p.AppendLine("body { width: auto !important;}");
            strbMensaje_p.AppendLine("table [class=main] { width: 93% !important;}");
            strbMensaje_p.AppendLine("table [class=full] { width: 100% !important; margin: 0px auto; } }");
            strbMensaje_p.AppendLine("</style>");
            strbMensaje_p.AppendLine("</head>");
            strbMensaje_p.AppendLine("<body yahoo='fix' leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'>");
            //Main Table Start
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' style='background:#fffff;'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'>");
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'>");
            strbMensaje_p.AppendLine("<table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' bgcolor='#ffffff' style='-moz-border-radius: 4px 4px 0px 0px; border-radius: 4px 4px 0px 0px;'>");
            strbMensaje_p.AppendLine("<table width='600' border='0' align='center' cellpadding='0' cellspacing='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td height='10' align='center' valign='top' style='line-height:25px; font-size:25px;'>&nbsp;</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' bgcolor='#fff'><table width='600' border='0' align='center' cellpadding='0' cellspacing='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='middle'>");
            strbMensaje_p.AppendLine("<img style='width: 100px' src=\"cid:logo\"/>");
            strbMensaje_p.AppendLine("</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            //Body
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='500' border='0' cellspacing='0' cellpadding='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' style='font-family:Open Sans, sans-serif, Verdana; font-size:16px;font-weight:bold; line-height:40px;color:#8fbf00;'>Notificación Automática</td>");
            strbMensaje_p.AppendLine("</tr><tr>");
            strbMensaje_p.AppendLine("<td align='left' valign='top' style='font-family:Open Sans, sans-serif, Verdana; font-size:14px;font-weight:normal; line-height:30px;color: #828282;'>");
            strbMensaje_p.AppendLine("<p>Este es un mensaje automático por la aplicación Momentos Don Fernando que indica que su nueva contraseña es: <strong>" + Password + "</strong>, este correo se genero el: ");
            strbMensaje_p.AppendLine("<strong>" + DateTime.Now + "</strong>, en caso de que usted no haya solicitado ese cambio comuníquese de inmediato con cualquier departamento de atención de Carnes Don Fernando para evitar que su información pueda verse comprometida.</p></td>");
            strbMensaje_p.AppendLine("</tr><tr>");
            strbMensaje_p.AppendLine("<td height='40' align='center' valign='top' style='font-size:40px; line-height:40px;'>&nbsp;</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            strbMensaje_p.AppendLine("</body>");
            strbMensaje_p.AppendLine("</html>");
            return strbMensaje_p.ToString(); //response cuerpo correo
        }

        public string SendMessage(string text) 
        {
            // mensaje del correo
            StringBuilder strbMensaje_p = new StringBuilder(); //constructor de cadena cuerpo de correo
            strbMensaje_p.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>"); //agrega encabezado
            strbMensaje_p.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            strbMensaje_p.AppendLine("<head>");
            strbMensaje_p.AppendLine("<meta http-equiv='X-UA-Compatible' content='IE=edge' />");
            strbMensaje_p.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            strbMensaje_p.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1' />");
            strbMensaje_p.AppendLine("<title>Notificación</title>");
            strbMensaje_p.AppendLine("<style type='text/css'>");
            strbMensaje_p.AppendLine("body {margin: 0; padding: 0; -webkit-text-size-adjust: none; -ms-text-size-adjust: none; background: #ffffff;}");
            strbMensaje_p.AppendLine("html {width: 100%;}");
            strbMensaje_p.AppendLine("table { border-spacing: 0; border-collapse: collapse;}");
            strbMensaje_p.AppendLine("table td { border-collapse: collapse; }");
            strbMensaje_p.AppendLine("@media only screen and (max-width:640px) {");
            strbMensaje_p.AppendLine("body { width: auto !important;}");
            strbMensaje_p.AppendLine("table [class=main] { width: 85% !important;}");
            strbMensaje_p.AppendLine("table [class=full] { width: 100% !important; margin: 0px auto;}");
            strbMensaje_p.AppendLine("table [class=two-left-inner] { width: 90% !important; margin: 0px auto; }}");
            strbMensaje_p.AppendLine("@media only screen and (max-width:479px) {");
            strbMensaje_p.AppendLine("body { width: auto !important;}");
            strbMensaje_p.AppendLine("table [class=main] { width: 93% !important;}");
            strbMensaje_p.AppendLine("table [class=full] { width: 100% !important; margin: 0px auto; } }");
            strbMensaje_p.AppendLine("</style>");
            strbMensaje_p.AppendLine("</head>");
            strbMensaje_p.AppendLine("<body yahoo='fix' leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'>");
            //Main Table Start
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' style='background:#fffff;'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'>");
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'>");
            strbMensaje_p.AppendLine("<table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' bgcolor='#ffffff' style='-moz-border-radius: 4px 4px 0px 0px; border-radius: 4px 4px 0px 0px;'>");
            strbMensaje_p.AppendLine("<table width='600' border='0' align='center' cellpadding='0' cellspacing='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td height='10' align='center' valign='top' style='line-height:25px; font-size:25px;'>&nbsp;</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' bgcolor='#fff'><table width='600' border='0' align='center' cellpadding='0' cellspacing='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='middle'>");
            strbMensaje_p.AppendLine("<img style='width: 100px' src=\"cid:logo\"/>");
            strbMensaje_p.AppendLine("</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            //Body
            strbMensaje_p.AppendLine("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='800' border='0' align='center' cellpadding='0' cellspacing='0' class='main'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top'><table width='500' border='0' cellspacing='0' cellpadding='0' class='two-left-inner'>");
            strbMensaje_p.AppendLine("<tr>");
            strbMensaje_p.AppendLine("<td align='center' valign='top' style='font-family:Open Sans, sans-serif, Verdana; font-size:16px;font-weight:bold; line-height:40px;color:#8fbf00;'>Notificación Automática</td>");
            strbMensaje_p.AppendLine("</tr><tr>");
            strbMensaje_p.AppendLine("<td align='left' valign='top' style='font-family:Open Sans, sans-serif, Verdana; font-size:14px;font-weight:normal; line-height:30px;color: #828282;'>");
            strbMensaje_p.AppendLine("<p>"+ text +"");
            strbMensaje_p.AppendLine("</p></td>");
            strbMensaje_p.AppendLine("</tr><tr>");
            strbMensaje_p.AppendLine("<td height='40' align='center' valign='top' style='font-size:40px; line-height:40px;'>&nbsp;</td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table></td>");
            strbMensaje_p.AppendLine("</tr></table>");
            strbMensaje_p.AppendLine("</body>");
            strbMensaje_p.AppendLine("</html>");
            return strbMensaje_p.ToString(); //response cuerpo correo
        }
    }
}
