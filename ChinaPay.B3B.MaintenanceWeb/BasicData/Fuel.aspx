<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fuel.aspx.cs" EnableSessionState="ReadOnly" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Fuel" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register Src="~/UserControl/Ariline.ascx" TagName="Ariline" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>燃油附加费维护</title>
</head>
<body>
    <form id="Form1" runat="server">
    <div class="contents">
     <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>燃油附加费维护</span>
    </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>
                                里程数：
                            </th>
                            <td>
                                <asp:TextBox ID="txtStartMileage" runat="server" CssClass="input1" ></asp:TextBox>
                            </td>
                            <th>
                                生效日期：
                            </th>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="input1"  onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                            </td>
                            <th>失效日期：</th>
                            <td>
                                <asp:TextBox ID="txtStopDate" runat="server" CssClass="input1" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>航空公司：</th>
                            <td>
                                <uc:Ariline runat="server"  ID="ucAriline" ClientIDMode="Static"/>
                            </td>
                            <td class="operator" colspan="4">
                                <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="button" 
                                    OnClientClick="return btnSubmit();" onclick="btnSelect_Click" />&nbsp;
                                <input type="button" name="button3" value="添加" class="button" onclick="javascript:window.location.href='Fuel_new.aspx?action=add'" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png"/>&nbsp;相应列表</dt>
                <dd>
                    <asp:GridView ID="gvFuel" runat="server" EmptyDataText="无满足条件的数据!"
                        AutoGenerateColumns="False" CssClass="tab3 list" onrowdeleting="gvFuel_RowDeleting" >
                        <Columns>  
                            <asp:TemplateField HeaderText="航空公司代码">
                                <ItemTemplate>
                                    <asp:Label ID="labAirlineCode" runat="server" Text='<%# Eval("AirlineCode.Value") %>' CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="航空公司简称">
                                <ItemTemplate>
                                    <asp:Label ID="labAirlineName" runat="server" Text='<%#Eval("Airline.ShortName") %>' CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Mileage" HeaderText="里程分界" />
                            <asp:BoundField DataField="Adult" HeaderText="成人价格" />
                            <asp:BoundField DataField="Child" HeaderText="儿童价格" />
                            <asp:BoundField DataField="EffectiveDate" HtmlEncode="false" DataFormatString="{0:yyyy-MM-dd}" HeaderText="生效日期" />
                            <asp:BoundField DataField="ExpiredDate" HtmlEncode="false" DataFormatString="{0:yyyy-MM-dd}" HeaderText="失效日期"/>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate> 
                                    <a href='Fuel_new.aspx?action=update&Id=<%#Eval("Id") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>' Text="删除" OnClientClick='return confirm("确定要删除吗?")'></asp:LinkButton>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright"><uc1:Pager ID="pagerl" runat="server"/></div>
                <div class="clear"></div> 
            </div>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script src="../Scripts/selector.js" type="text/javascript"></script>
<script src="../Scripts/airport.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    pageName = "Fuel";
    $("#btnSelect").click(function () { SaveSearchCondition(pageName); });
    function btnSubmit() {
        var txtStartMileage = $("#txtStartMileage").val();
        var regu = /^[0-9]*$/;
        if (txtStartMileage != "" && !($.trim(txtStartMileage).match(regu))) {
            alert("里程数格式不正确!");
            $("#txtStartMileage").select();
            return false;
        }
    }
</script>