using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class Legajos
    {
        private string _nombreCompleto;
        public string NombreCompleto
        {
            get
            {
                return Helper.ToCapitalize(this.Apellido.ToLower()) + ", " + Helper.ToCapitalize(this.Nombre.ToLower());
            }
            set
            {
                _nombreCompleto = value;
            }
        }



        public string PrestacionEmergencia
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                {
                    return this.objEmpresaLegajo.PrestacionEmergencia;
                }
                return string.Empty;
            }
        }

        public string AutorizadoConducir
        {
            get
            {
                if (this.AutorizadoCond.HasValue && this.AutorizadoCond.Value)
                {
                    if (this.FechaVencimientoCarnet.HasValue && this.FechaVencimientoCarnet.Value > DateTime.Now)
                        return "SI  (Vto: " + this.FechaVencimientoCarnet.Value.ToShortDateString() + ")";
                    else
                        return "NO";
                }
                return "NO";
            }
        }

        public string Empresa
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                    return this.objEmpresaLegajo.RazonSocial;
                else
                    return string.Empty;
            }
        }

        public string Contratista
        {
            get
            {
                if (this.objEmpresaLegajo != null)
                    return this.objEmpresaLegajo.RazonSocial;
                else
                    return string.Empty;
            }
        }

        public string RAC1
        {
            get
            {
                CursosLegajos cur = this.CursosLegajos.FirstOrDefault();
                if (cur != null)
                {
                    return cur.objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

        public string RAC2
        {
            get
            {
                if (this.CursosLegajos.Count > 1)
                {
                    return this.CursosLegajos.ToList()[1].objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

        public string RAC3
        {
            get
            {
                if (this.CursosLegajos.Count > 2)
                {
                    return this.CursosLegajos.ToList()[2].objCurso.Descripcion;
                }
                return string.Empty;
            }
        }

        public string getDescripcionSeguro
        {
            get
            {
                //<sessionState mode="SQLServer" timeout="20" allowCustomSqlDatabase="true" sqlConnectionString="Data Source=localhost;Initial Catalog=wi871133_ASPState;Integrated Security=SSPI;" cookieless="false" />

                if (this.NroPoliza != null && this.NroPoliza.Trim() != "" && this.CompañiaSeguro != null)
                {
                    //lblSeguro.Value = "Acc Per:";
                    return this.NroPoliza + " - " + this.objCompañiaSeguro.Descripcion.Trim();
                }
                else
                {
                    //lblSeguro.Value = "ART:";
                    return this.objEmpresaLegajo.DescArt;
                }
            }

        }

        public string PrimerVencimientoCredencial
        {
            get
            {

                List<DateTime?> allFechasCalculo = new List<DateTime?>();
                DateTime? fechaVencimientoContrato = null;
                DateTime? fechaAltaMedica = this.FechaUltimoExamen != null ? this.FechaUltimoExamen.Value.AddYears(1) : (DateTime?)null;

                if (!this.CursosLegajos.IsLoaded) this.CursosLegajos.Load();
                DateTime? fechaRac = this.CursosLegajos.Min(w => w.FechaVencimiento);
                
                if (!this.objContEmpLegajos.IsLoaded) this.objContEmpLegajos.Load();
                var contratoActivo = this.objContEmpLegajos.Where(w => w != null && w.IdLegajos == this.IdLegajos).LastOrDefault();
                if (contratoActivo != null)
                {

                    DateTime? fechaSeguro = this.CompañiaSeguro != null && this.FechaVencimiento != null ? this.FechaVencimiento.Value : (DateTime?)null;
                    if (fechaSeguro == null)
                    {

                        Seguros segART = this.objContEmpLegajos.Last().ContratoEmpresas.Empresa.Seguros.Where(w => w.objTipoSeguro != null && w.objTipoSeguro.Descripcion.Contains("ART")).FirstOrDefault();
                        if (segART != null)
                            fechaSeguro = segART.FechaVencimiento;

                    }

                    fechaVencimientoContrato = contratoActivo.ContratoEmpresas.Contrato.Prorroga.HasValue && contratoActivo.ContratoEmpresas.Contrato.Prorroga.Value > contratoActivo.ContratoEmpresas.Contrato.FechaVencimiento ? contratoActivo.ContratoEmpresas.Contrato.Prorroga : contratoActivo.ContratoEmpresas.Contrato.FechaVencimiento;

                    allFechasCalculo.Add(fechaAltaMedica);
                    allFechasCalculo.Add(fechaRac);
                    allFechasCalculo.Add(fechaSeguro);
                    allFechasCalculo.Add(fechaVencimientoContrato);
                    allFechasCalculo.Add(this.CredVencimiento);

                    DateTime minFecha = allFechasCalculo.Where(w => w != null).Min(w => w.Value);

                    return minFecha.ToShortDateString();
                }
                else
                    return DateTime.Now.ToShortDateString();
               
            }

        }

    }
}