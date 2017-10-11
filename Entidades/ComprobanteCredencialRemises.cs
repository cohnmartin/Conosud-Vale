namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;

    /// <summary>
    /// Summary description for InformeAltaBajaLegajo.
    /// </summary>
    public partial class ComprobanteCredencialRemises : Telerik.Reporting.Report
    {
        public ComprobanteCredencialRemises()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static string Greet(string name)
        {
            return string.Format("Hello, {0}", name);
        }


        public void InitReport(object img, List<CursosLegajos> Cursos, Legajos Leg)
        {
            pictureBox5.Value = img;

            txtFechaImpresion.Value = "Fecha Emisión:"  + DateTime.Now.ToShortDateString();
            txtFechaImpresion1.Value = "Fecha Emisión:" + DateTime.Now.ToShortDateString();

            //if (Leg.NroPoliza!= null &&  Leg.NroPoliza.Trim() != "" && Leg.CompañiaSeguro != null)
            //{
            //    lblSeguro.Value = "Acc Per:";
            //    txtSeguro.Value = Leg.NroPoliza + " - " + Leg.objCompañiaSeguro.Descripcion.Trim();
            //}
            //else
            //{
            //    lblSeguro.Value = "ART:";
            //    txtSeguro.Value = Leg.objEmpresaLegajo.DescArt;
            //}

        }
    }
}