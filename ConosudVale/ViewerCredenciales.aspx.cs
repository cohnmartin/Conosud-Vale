using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Drawing.Imaging;
using System.Drawing;

public partial class ViewerCredenciales : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;

    public EntidadesConosud Contexto
    {
        //get
        //{
        //    if (Session["Contexto"] != null)
        //        return (EntidadesConosud)Session["Contexto"];
        //    else
        //    {
        //        Session["Contexto"] = new EntidadesConosud();
        //        return (EntidadesConosud)Session["Contexto"];
        //    }
        //}

        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["IdLegajo"] != null)
                {
                    
                    long id = long.Parse(Request.QueryString["IdLegajo"].ToString());

                    List<Legajos> Legs = (from U in Contexto.Legajos.Include("objEmpresaLegajo").Include("objEmpresaLegajo.Seguros")
                                          .Include("objCompañiaSeguro").Include("objEmpresaLegajo.Seguros.objTipoSeguro")
                                          .Include("objEmpresaLegajo.Seguros.objCompañia")
                                          where U.IdLegajos == id select U).ToList();

                    ComprobanteCredencial rep = null;
                    ComprobanteCredencialRemises rep1 = null;
                    List<CursosLegajos> cursos = Legs.First().CursosLegajos.Where(c => c.Aprobado).ToList();
                    
                    /// Esto lo hago para obligar a cargar todos los objetos, ya que en producción dado a que se utiliza
                    /// sesiones en base de datos, no se porque con lo reportes no carga bien los objetos.
                    string a =  Legs.First().objEmpresaLegajo.DescArt;
                    bool empresaRemises = Legs.First().objEmpresaLegajo.RazonSocial.ToUpper().Contains("BRISAS");
                    
                    if (Legs.First().RutaFoto != null)
                    {
                        string ruta = Server.MapPath("ImagenesLegajos") + "/" + Legs.First().RutaFoto;
                        Bitmap imgLegajo = new Bitmap(ruta);

                        if (!empresaRemises)
                        {
                            rep = new ComprobanteCredencial();
                            rep.InitReport(imgLegajo, cursos, Legs.First());
                        }
                        else
                        {
                            rep1 = new ComprobanteCredencialRemises();
                            rep1.InitReport(imgLegajo, cursos, Legs.First());
                        }
                    }
                    else
                    {
                        if (!empresaRemises)
                        {
                            rep = new ComprobanteCredencial();
                            rep.InitReport(null, cursos, Legs.First());
                        }
                        else
                        {
                            rep1 = new ComprobanteCredencialRemises();
                            rep1.InitReport(null, cursos, Legs.First());
                        }
                    }

                    if (!empresaRemises)
                    {
                        rep.DataSource = Legs;
                        this.ReportViewer1.Report = rep;
                    }
                    else
                    {
                        rep1.DataSource = Legs;
                        this.ReportViewer1.Report = rep1;
                    }
                    

                    this.ReportViewer1.RefreshReport();

                }
                else if (Request.QueryString["IdVehiculoEquipo"] != null)
                {
                    long id = long.Parse(Request.QueryString["IdVehiculoEquipo"].ToString());
                    List<VahiculosyEquipos> VahiyEqui = (from U in Contexto.VahiculosyEquipos
                                                             .Include("objTipoUnidad")
                                                             .Include("objEmpresa") 
                                                         where U.IdVehiculoEquipo == id select U).ToList();


                    ComprobanteCredencialVehiculoEquipo rep = new ComprobanteCredencialVehiculoEquipo();
                    rep.DataSource = VahiyEqui;
                    this.ReportViewer1.Report = rep;
                    //VahiculosyEquipos a;  a.NroHabilitacion
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.StackTrace.ToString());
            }

            
            //if (this.Session["TipoUsuario"].ToString() == "Cliente")
            //    ReportViewer1.ShowExportGroup = false;
        }
    }
}