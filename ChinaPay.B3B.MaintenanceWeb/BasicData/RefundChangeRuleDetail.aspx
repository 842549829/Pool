<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundChangeRuleDetail.aspx.cs"
    Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.RefundChangeRuleDetail" %>

<%@ Register Src="../UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>航空公司退改签约定查询</title>
    <style type="text/css">
        .rules
        {
            padding:10px 20px;
            background-color:#EEEEEE;
            font-family: "微软雅黑";
            font-size:1.2em;
        }
        .rules li
        {
            padding:5px;
            font-weight: 900;
        }
        .rules li span
        {
            font-weight: 200;
            display: inline-block;
        }
        .rules #phone
        {
            float:right;
            font-size:1.2em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>航空公司退改签规定</span>
        </div>
        <div>
            <ul class="rules box-a">
                <li>航空公司<asp:Label Text="" ID="lblAirline" runat="server" /></li>
                <li>使用条件：<asp:Label Text="" ID="lblCondition" runat="server" /></li>
                <li>废票规定：<asp:Label Text="" ID="lblScrapRules" runat="server" /></li>
                <li>升舱规定：<asp:Label Text="" ID="lblUpgradRules" runat="server" /></li>
                <li>备注：<asp:Label Text="" ID="lblRemark" runat="server" /></li>
                <li>      
                    <input type="button" class="button" style="width:100px;" value="修改基础信息" onclick="location.href='/BasicData/RefundChangeTicketNewUpdate.aspx?action=update&code=<%=AirlineCode %>'" />  <div id="phone">航空公司电话<asp:Label Text="" ID="lblPhone" runat="server" /></div>   </li>
            </ul>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" />&nbsp;航空公司退改签规定</dt>
                <dd>
                    <asp:GridView ID="gvRefundRules" runat="server" AutoGenerateColumns="False" CssClass="tab3 list"
                        EmptyDataText="无满足条件的数据!" OnRowDeleting="gvChildTicketClassInfo_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Bunks" HeaderText="舱位" />
                            <asp:BoundField DataField="ScrapBefore" HeaderText="离站前退票规定" />
                            <asp:BoundField DataField="ScrapAfter" HeaderText="离站后退票规定" />
                            <asp:BoundField DataField="ChangeBefore" HeaderText="离站前改期规定" />
                            <asp:BoundField DataField="ChangeAfter" HeaderText="离站后改期规定" />
                            <asp:BoundField DataField="Endorse" HeaderText="签转规定" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <a href='/BasicData/RefundAndReschedulingDetati.aspx?action=update&Id=<%#Eval("Id") %>&airline=<%#Eval("Airline") %>'>编辑</a>
                                    <asp:LinkButton ID="linkDel" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id")%>'
                                        Text="删除" OnClientClick='return confirm("确定要删除吗?")'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="searchListTitle" />
                    </asp:GridView>
                </dd>
                <dd style="text-align: center;">
                    <input type="button" value="添加" class="button" onclick="location.href='/BasicData/RefundAndReschedulingDetati.aspx?action=add&airline=<%=AirlineCode %>'" />
                    <input type="button" value="返回" class="button" onclick="location.href='RefundChangeRuleList.aspx'" />
                </dd>
            </dl>
            
            <div class="wpager">
                <div class="wpageright">
                    <uc1:Pager ID="Pager1" runat="server" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../js/forms.js"></script>
<script language="javascript" type="text/javascript">
    function btnSubmit()
    {
        var regu = /^[A-Za-z]{1,2}$/;
        var code = $("#txtCwCode").val();
        if (code != "" && !(code.length == 1 || code.length == 2) && !($.trim(code).match(regu)))
        {
            alert("舱位格式不正确！");
            return false;
        }
    }  
</script>
