using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class Empresa
    {

        public string DescArt
        {
            get
            {
                
                if (this.Seguros != null)
                {

                    Seguros segART = this.Seguros.Where(w => w.objTipoSeguro != null && w.objTipoSeguro.Descripcion.Contains("ART")).FirstOrDefault();
                    if (segART != null)
                    {
                        if (segART.objCompañiaReference != null)
                            //return segART.NroPoliza + " - " + segART.objCompañia.Descripcion;
                            return segART.objCompañia.Descripcion;
                        else
                            return "Sin Asignar";
                    }
                    else
                        return "Sin Asignar no econtro ART";
                }
                else
                    return "Sin Asignar no tiene el seguro la empresa";
            }
        }

        public string DescVida
        {
            get
            {
                Seguros segART = this.Seguros.Where(w => w.objTipoSeguro != null && w.objTipoSeguro.Descripcion.Contains("Vida")).FirstOrDefault();
                if (segART != null)
                {
                    //return segART.NroPoliza + " - " + segART.objCompañia != null ? segART.objCompañia.Descripcion : "";
                    return segART.objCompañia != null ? segART.objCompañia.Descripcion : "Sin Asignar";
                }
                else
                    return "Sin Asignar";
            }
        }

    }
}
