<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendRecord.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSSendRecord" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>短信记录查询</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="/Styles/bank.css" rel="stylesheet" type="text/css" />

<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        短信记录查询</h3>
    <div class="SMS-nav-box">
        <p>
            请选择您的查询类型</p>
        <ul class="SMS-nav clearfix" id="send_ul">
            <li class="curr">发送记录</li>
            <li>购买记录</li>
        </ul>
    </div>
    <div class="box-a">
        <div class="condition">
            <table id="send">
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">接收号码：</span>
                            <asp:TextBox runat="server" CssClass="text" ID="txtPhone"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">发送状态：</span>
                            <asp:DropDownList ID="drpStatus" runat="server">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>已提交</asp:ListItem>
                                <asp:ListItem>发送失败</asp:ListItem>
                                <asp:ListItem>已发送</asp:ListItem>
                                <asp:ListItem>部分发送失败</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">发送时间：</span>
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtSendStartTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtSendEndTime\')||\'%y-%M-%d\'}', readOnly:true })"></asp:TextBox>
                            至
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtSendEndTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtSendStartTime\')}', maxDate: '#F{\'%y-%M-%d\'}', readOnly:true})"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button runat="server" CssClass="btn class1" Text="查询" ID="btnSendQuery" OnClick="btnSendQuery_Click" />
                    </td>
                </tr>
            </table>
            <table id="buy">
                <tr>
                    <td colspan="3">
                        <div class="input">
                            <span class="name">发送时间：</span>
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtBuyStartTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, maxDate: '#F{$dp.$D(\'txtBuyEndTime\')||\'%y-%M-%d\'}', readOnly:true })"></asp:TextBox>
                            至
                            <asp:TextBox runat="server" CssClass="text text-s1" ID="txtBuyEndTime" onfocus="WdatePicker({ skin: 'default', isShowClear: false, minDate: '#F{$dp.$D(\'txtBuyStartTime\')}', maxDate: '#F{\'%y-%M-%d\'}', readOnly:true})"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="btns">
                        <asp:Button runat="server" CssClass="btn class1" Text="查询" ID="btnBuyQuery" OnClick="btnBuyQuery_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_sendRecord" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="接收号码">
                    <ItemStyle CssClass="w10" />
                    <ItemTemplate>
                        <%#Eval("Mobiles")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="短信内容" DataField="Content" />
                <asp:BoundField HeaderText="发送时间" DataField="SendTime" ItemStyle-CssClass="w12" />
                <asp:TemplateField HeaderText="发送状态">
                    <ItemStyle CssClass="w10" />
                    <ItemTemplate>
                        <%#Eval("Status")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="失败号码">
                    <ItemStyle CssClass="w10" />
                    <ItemTemplate>
                        <%#Eval("FiadPhone")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作员" DataField="OperatorAccount" ItemStyle-CssClass="w10" />
            </Columns>
        </asp:GridView>
        <div class="btns">
            <uc:Pager runat="server" ID="send_pager" Visible="false"></uc:Pager>
        </div>
        <asp:GridView ID="grv_buyRecord" runat="server" CssClass="nfo-table info" Width="100%"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="购买时间" DataField="PayTime" ItemStyle-CssClass="w20" />
                <asp:BoundField HeaderText="购买金额/积分" DataField="TotalAmount" />
                <asp:BoundField HeaderText="购买条数" DataField="TotalCount" />
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="buy_pager" Visible="false"></uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
    </div>
    <asp:HiddenField ID="hidli" runat="server" Value="0" />
    <asp:HiddenField ID="hidSendId" runat="server" />
    <asp:Button runat="server" CssClass="hidden" Text="发送" ID="btnReSend" OnClick="btnReSend_Click" />
    </form>
</body>
</html>
<script src="/Scripts/Global.js" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#hidli").val() == "0") {
            $("#send").show();
            $("#buy").hide();
            $("#send_ul li").removeClass("curr");
            $("#send_ul li").eq(0).addClass("curr");
        } else {
            $("#send").hide();
            $("#buy").show();
            $("#send_ul li").removeClass("curr");
            $("#send_ul li").eq(1).addClass("curr");
        }
        $("#send_ul li").click(function () {
            if ($(this).attr("class") != "curr") {
                $("#send_ul li").removeClass("curr");
                $(this).addClass("curr");
                if ($.trim($(this).html()) == "发送记录") {
                    $("#hidli").val("0");
                    $("#btnSendQuery").click();
                }
                if ($.trim($(this).html()) == "购买记录") {
                    $("#hidli").val("1");
                    $("#btnBuyQuery").click();
                }
            }
        });
        $("#btnReSend").click(function () {
            if ($("#hidSendId").val() == "") {
                alert("无法重新发送");
            }
        });
    });
    function send(id) {
        $("#hidSendId").val(id);
        $("#btnReSend").click();
    }
</script>
