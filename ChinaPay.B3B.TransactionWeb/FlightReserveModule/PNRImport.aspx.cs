using System;
using System.Text.RegularExpressions;
using System.Web;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class PNRImport : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                hfdServicePhone.Value = CurrenContract.ServicePhone;
                hfdEnterpriseQQ.Value = CurrenContract.EnterpriseQQ;
            }
        }

        protected void btnPNRCodeImport_Click(object sender, EventArgs e) {
            bool needImputPat = false;
            PnrImportResult excResult = null;
            try {
                if(this.radChildrenPNR.Checked) {
                    excResult = ImportHelper.ImportByPNRCode(this.txtAdultPNRCode.Text.Trim(), this.txtChildrenPNRCode.Text.Trim(), Common.Enums.PassengerType.Child, txtPATContent.Value, HttpContext.Current);
                    needImputPat = excResult.NeedPAT;
                } else {
                    excResult =ImportHelper.ImportByPNRCode(this.txtPNRCode.Text.Trim(), string.Empty, Common.Enums.PassengerType.Adult, txtPATContent.Value, HttpContext.Current);
                    needImputPat = excResult.NeedPAT;
                }
                txtPNRContent.Text = excResult.PNRContent;
            } catch(Exception ex) {
                showWarnInfo(ex.Message);
                //ShowExceptionMessage(ex, "PNR编码导入");
                return;
            }
            if (needImputPat) showPatInput();
            else gotoNextStep();
        }

        protected void btnPNRContentImport_Click(object sender, EventArgs e) {
            bool needImputPat = false;
            try {
                if(this.radChildrenPNRContent.Checked) {
                   needImputPat=  ImportHelper.ImportByPNRContent(this.txtPNRContent.Text, this.txtAdultPNRCodeForContent.Text.Trim(), Common.Enums.PassengerType.Child, HttpContext.Current).NeedPAT;
                } else {
                    needImputPat = ImportHelper.ImportByPNRContent(this.txtPNRContent.Text, string.Empty, Common.Enums.PassengerType.Adult, HttpContext.Current).NeedPAT;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "PNR内容导入");
                return;
            }
            if (needImputPat) showPatInput();
            else gotoNextStep();
        }
        private void gotoNextStep() {
            RegisterScript("window.top.location='/FlightReserveModule/ChoosePolicyWithImport.aspx?source=" + ChoosePolicy.ImportSource + "';");
        }

        private void showPatInput()
        {
            RegisterScript("$(function(){setTimeout(function(){$(\"#divOpcial\").click();},200);})"); 
        }

        private void showWarnInfo(string message)
        {
            RegisterScript("$(function(){setTimeout(function(){$(\"#lblWarnInfo\").html('"+message+"');$(\"#divWarnInfo\").click();},200);})"); 
        }

        protected string PatReg
        {
            get {
                return Regex.Replace(Service.Command.RegexUtil.PATCmdRegex, @"\?\<.+\>|\(\?:.+\)", string.Empty);
            }
        }
    }
}