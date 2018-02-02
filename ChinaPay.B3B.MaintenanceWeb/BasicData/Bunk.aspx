<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bunk.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Bunk" EnableSessionState="ReadOnly" %>
<%@ Register src="../UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>舱位代码维护</title>
    <script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
</head>
<body>
    <form id="form1" runat="server" >
    <div class="contents">
    <div class="breadcrumbs">
        <span>当前位置:</span><span>基础数据管理</span>»<span>舱位折扣维护</span>
    </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" alt=""/>&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search condition">
                        <tr>
                            <th>航空公司：</th>
                            <td><asp:DropDownList ID="ddlAirline" CssClass="input1" runat="server"></asp:DropDownList></td>
                            <th>出发机场：</th>
                            <td><asp:DropDownList ID="ddlDeparture" CssClass="input1" runat="server"></asp:DropDownList></td>
                            <th>到达机场：</th>
                            <td><asp:DropDownList ID="ddlArrival" CssClass="input1" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <th>舱位代码：</th>
                            <td><asp:TextBox ID="txtCwCode" CssClass="input1" runat="server" ></asp:TextBox></td>
                            <th>舱位状态：</th>
                            <td>
                                <asp:DropDownList ID="ddlStatus" CssClass="input1" runat="server">
                                    <asp:ListItem Value="">不限</asp:ListItem>
                                    <asp:ListItem Value="True">启用</asp:ListItem>
                                    <asp:ListItem Value="False">禁用</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <th>
                                舱位类型：
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="dropBunk" CssClass="input1"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>航班开始日期：</th>
                            <td>
                                <asp:TextBox ID="txtStartTime" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                            </td>
                            <th>航班截止日期：</th>
                            <td>
                                <asp:TextBox ID="txtStopTime" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:true,readOnly:true})"></asp:TextBox>
                            </td>
                            <th>
                                适用行程类型：
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="dropVoyageType" CssClass="input1"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="6">
                                <asp:Button ID="btnRefresh" runat="server" Text="刷新缓存" CssClass="button" Width="120px"
                                    onclick="btnRefresh_Click" />&nbsp;
                                <asp:Button ID="btnSelect" runat="server" Text="查询"  CssClass="button"   
                                    OnClientClick="return btnSubmit()" onclick="btnSelect_Click" />&nbsp;
                                <input type="button" onclick="javascript:window.location.href='Bunk_new.aspx?action=add'" class="button" value="添加" name="button3"/>
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
       <div class="title">
        <dl>
            <dt><img src="../images/icon.png"/>&nbsp;相应列表</dt>
            <dd>
              <asp:GridView ID="gvDiscount" runat="server" AutoGenerateColumns="False" 
                    CssClass="tab3 list" onrowcommand="gvDiscount_RowCommand" EmptyDataText="无满足条件的数据!"
                    onrowdeleting="gvDiscount_RowDeleting">
                        <Columns>  
                            <asp:BoundField DataField="ETDZDate"  HeaderText="出票日期"/>
                            <asp:BoundField DataField="FlightBeginDate"  HeaderText="航班开始日期"/>
                            <asp:BoundField DataField="FlightEndDate"  HeaderText="航班截止日期"/>
                            <asp:BoundField DataField="AirlineCode"  HeaderText="航空公司"/>
                            <asp:BoundField DataField="BunkCode"  HeaderText="舱位代码"/>
                            <asp:TemplateField HeaderText="适用行程">
                                <ItemTemplate>
                                    <%#Eval("VoyageType")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="状态">
                                <ItemTemplate> 
                                    <asp:Label ID="lblValid" runat="server" Text='<%# GetState(Eval("Valid").ToString()) %>' CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                   <asp:TemplateField HeaderText="类型">
                                <ItemTemplate> 
                                    <asp:Label ID="lblBunkType" runat="server" Text='<%# Eval("BunkType") %>' CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="出发机场">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("DepartAriport") %>' ID="lblDepartureCode" CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="到达机场">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("ArriveAriport") %>' ID="lblAirlineCode" CssClass="fontColor01" Font-Bold="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate> 
                                    <asp:LinkButton ID="linkOprate"  CommandName="opdate"  CommandArgument='<%# Eval("Id")%>' runat="server" Text='<%# ButtonState(Eval("Valid").ToString())%>' OnClientClick='return confirm("确定要修改状态吗?")'></asp:LinkButton>
                                    <a href='Bunk_new.aspx?action=update&Id=<%#Eval("Id") %>'>修改</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>'  Text="删除"  OnClientClick='return confirm("确定要删除吗?")'></asp:LinkButton>                                   
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
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/SaveCondition.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    pageName = "Bunk";
    $("#btnSelect").click(function () { SaveSearchCondition(pageName); });
    function btnSubmit() {
        var regu = /^[A-Za-z]{1,2}$/;
        var code = $("#txtCwCode").val();
        if (code != "" && !(code.length == 1 || code.length == 2) && !($.trim(code).match(regu))) {
            alert("舱位代码格式不正确！");
            return false;
        }

        var strat = $("#txtStartTime").val();
        var stop = $("#txtStopTime").val();
        if (strat != "" && stop != "") {
            if (stop < strat) {
                alert("航班截止时间应该在开始时间之后");
                $("#txtStopTime").select();
                return false;
            }
        }
    }  
</script>