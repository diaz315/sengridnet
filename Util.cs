using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace SendGridNet
{
    public class Util
    {
        public static bool ValidarEmail(string email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static List<Persona> LeerCsv(string path)
        {
            string[] csv = File.ReadAllLines(path);

            List<Persona> listaData = new List<Persona>();

            for (int i = 0;i<csv.Length;i++)
            {
                if (i == 0)
                    continue;

                var data = csv[i].Split(';');

                var resultado = new Persona
                {
                    nombre = data[0],
                    correo = data[1],
                    codigo = data[2],
                };

                if (ValidarEmail(resultado.correo))
                {
                    listaData.Add(resultado);
                }
                else {
                    //throw new Exception("Email no valido");
                }
            }
            return listaData;

        }

        public static string GetMac() {
            return
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        }

    }
}
