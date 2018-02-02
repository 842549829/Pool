using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOEMSiteSetHeader : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                OEMInfo oem = null;
                if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
                {
                    oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
                    hidValue.Value = "1";
                }
                else
                {
                    oem = OEMService.QueryOEM(Guid.Parse(Request.QueryString["id"]));
                    hidValue.Value = "2";
                    btnCancel.Visible = false;
                }
                if (oem != null)
                {
                   hidOemdId.Value = oem.Id.ToString();
                    txtKeyWord.Value = oem.Setting.SiteKeyWord;
                    txtKeyDes.Value = oem.Setting.SiteDescription;
                    txtCopyright.Value = oem.Setting.CopyrightInfo;
                    txtBGColor.Value = oem.Setting.BGColor;
                    txtShowBGColor.Style.Add("background-color", "#" + (oem.Setting.BGColor.Replace("#", "")));
                    string strheader = "";
                    if (oem.Setting.HeaderLinks != null)
                    {
                        int countheader = oem.Setting.HeaderLinks.Count();
                        int countfooter = oem.Setting.FooterLinks.Count();
                        int index = 0;
                        foreach (var item in oem.Setting.HeaderLinks)
                        {
                            index++;
                            if (strheader == "")
                            {
                                if (countheader == 1)
                                {
                                    strheader += "<span>增加头部链接：</span><div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add'>+</a></div>";
                                }
                                else
                                {
                                    strheader += "<span>增加头部链接：</span><div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a></div>";
                                }
                            }
                            else
                            {
                                if (index == 5)
                                {
                                    strheader += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add' style='visibility:hidden;'>+</a></div>";
                                }
                                else
                                {
                                    if (countheader == index)
                                    {
                                        strheader += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add' >+</a></div>";
                                    }
                                    else
                                    {
                                        strheader += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a></div>";
                                    }
                                }
                                //else
                                //{
                                //    strheader += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a><a class='add' style='visibility:hidden;'>+</a></div>";
                                //}
                            }
                        }
                        string strfooter = "";
                        index = 0;
                        foreach (var item in oem.Setting.FooterLinks)
                        {
                            index++;
                            if (strfooter == "")
                            {
                                if (countfooter == 1)
                                {
                                    strfooter += "<span>底部链接管理：</span><div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add'>+</a></div>";
                                }
                                else
                                {
                                    strfooter += "<span>底部链接管理：</span><div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a></div>";
                                }
                            }
                            else
                            {
                                if (index == 5)
                                {
                                    strfooter += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add' style='visibility:hidden;'>+</a></div>";
                                }
                                else
                                {
                                    if (countfooter == index)
                                    {
                                        strfooter += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='add' >+</a></div>";
                                    }
                                    else
                                    {
                                        strfooter += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a></div>";
                                    }
                                }

                                //else
                                //{
                                //    strfooter += "<div><input type='text' placeholder='链接名称' value='" + item.LinkName + "' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='" + item.URL + "'  class='text' />\n<a class='reduce'>-</a></div>";
                                //}
                            }
                        }
                        if (strheader != "")
                        {
                            divHeader.InnerHtml = strheader;
                        }
                        if (strfooter != "")
                        {
                            divFooter.InnerHtml = strfooter;
                        }
                    }
                }
            }
        }
    }
}