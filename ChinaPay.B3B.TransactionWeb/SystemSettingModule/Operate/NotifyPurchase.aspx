<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotifyPurchase.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.NotifyPurchase" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        航班变更通知</h3>
    <div class="table mar">
        <p class="box">
            采购账号：<asp:Label ID="lblPurchaseNo" runat="server"></asp:Label>(<asp:Label ID="lblPurchaseName"
                runat="server"></asp:Label>) <span class="pad-l">联系电话：</span><asp:Label ID="lblPurchaseContactPhone"
                    runat="server"></asp:Label>
            <span class="pad-l">联系QQ：</span><asp:Label ID="lblPurchaseContactQQ" runat="server"></asp:Label>
            <span class="pad-l">受影响订单数</span><asp:Label ID="lblPurchaseOrderCount" runat="server"></asp:Label>
            <span class="pad-l">受影响航班数</span><asp:Label ID="lblPurchaseFlightCount" runat="server"></asp:Label>
        </p>
        <asp:Repeater ID="rptNotify" runat="server">
            <HeaderTemplate>
                <table class="mar-t">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th class="w8">
                                PNR
                            </th>
                            <th class="w10">
                                订单号
                            </th>
                            <th class="w8">
                                原航空公司
                            </th>
                            <th class="w8">
                                新航空公司
                            </th>
                            <th class="w8">
                                变更类型
                            </th>
                            <th class="w8">
                                原航班号
                            </th>
                            <th class="w8">
                                新航班号
                            </th>
                            <th class="w10">
                                原起飞时间
                            </th>
                            <th class="w10">
                                新起飞时间
                            </th>
                            <th class="w10">
                                原到达时间
                            </th>
                            <th class="w10">
                                新到达时间
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <input type="checkbox" pnr='<%#Eval("PNR") %>' orderid='<%#Eval("OrderId") %>' id='<%#Eval("TransferId") %>' />
                    </td>
                    <td>
                        <span class="obvious">
                            <%#Eval("PNR")%></span>
                    </td>
                    <td>
                        <a href='../../OrderModule/Operate/OrderDetail.aspx?id=<%#Eval("OrderId") %>'>
                            <%#Eval("OrderId")%></a>
                    </td>
                    <td>
                        <%#Eval("OriginalCarrierName")%>
                    </td>
                    <td>
                        <%#Eval("CarrierName")%>
                    </td>
                    <td>
                        <%#Eval("TransferType")%>
                    </td>
                    <td>
                        <%#Eval("OriginalFlightNo")%>
                    </td>
                    <td>
                        <%#Eval("FlightNo")%>
                    </td>
                    <td>
                        <%#Eval("OriginalTakeoffTime", "{0:yyyy-MM-dd HH:mm}")%>
                    </td>
                    <td>
                        <%#Eval("TakeoffTime", "{0:yyyy-MM-dd HH:mm}")%>
                    </td>
                    <td>
                        <%#Eval("OriginalArrivalTime","{0:yyyy-MM-dd HH:mm}")%>
                    </td>
                    <td>
                        <%#Eval("ArrivalTime", "{0:yyyy-MM-dd HH:mm}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <p class="total-table" id="pInfo" runat="server">
            <input type="checkbox" id="check_All" onclick="checkAll(this);" /><label class="pad-l">全选</label><a
                href="#" class="mar-l a-btn" id="copyPnr">复制所选PNR</a><a href="#" class="mar-l a-btn" id="copyOrderId">复制所选订单号</a></p>
        <div class="btns">
            <uc:Pager ID="pager" runat="server" Visible="false" />
        </div>
        <div class="btns">
            <input type="button" class="btn class1" id="btnNoticeHandler" value="通知处理" />
            <input type="button" class="btn class2" id="btnBack" value="返&nbsp;&nbsp;回" onclick="javascript:window.location.href='FlightChangeNotice.aspx';" />
        </div>
    </div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divNoticeHandler'}">
    </a>
    <div class="layer4 hidden" id="divNoticeHandler">
        <h4>
            航班变更通知记录<a href="#" class="close">关闭</a></h4>
        <p class="handle">
            <asp:RadioButton ID="radioMessage" runat="server" Checked="true" GroupName="type"
                CssClass="mar-l" Text="发送短信给采购" />
            <asp:RadioButton ID="radioHandler" runat="server" GroupName="type" CssClass="mar-l"
                Text="已人工处理" />
        </p>
        <div class="handle-box" id="divMessage">
            <textarea class="text textarea_width" runat="server" id="txtMessage" cols="10" rows="5"></textarea>
            <p class="txt-r">
                超过61个字将分条发送，已输入<span class="obvious font-b" id="messageCount">0</span>字</p>
        </div>
        <div class="handle-box" id="divPersonHandler">
            <p>
                采购账号：<asp:Label ID="lblPurchaseAccountNo" runat="server"></asp:Label></p>
            <p>
                通知方式：<asp:DropDownList ID="ddlNoticeWay" runat="server">
                </asp:DropDownList>
            </p>
            <p>
                通知结果：<asp:DropDownList ID="ddlNoticeResult" runat="server">
                </asp:DropDownList>
            </p>
            <p>
                通知备注：<asp:TextBox ID="txtNoticeRemark" runat="server" class="text text_width" /></p>
        </div>
        <div class="btns">
            <asp:Button ID="btnSubmit" runat="server" Text="提&nbsp;&nbsp;交" CssClass="btn class1"
                OnClick="btnSubmit_Click" />
            <button class="btn class2 close" id="btnCancel" type="button">
                取&nbsp;&nbsp;消</button>
        </div>
    </div>
    <a id="divOption" style="display: none;" data="{type:'pop',id:'divCopy'}"></a>
    <div class="layer4 hidden" id="divCopy">
        <h4>
            复制PNR或订单号<a href="#" class="close">关闭</a></h4>
        <div class="handle-box">
            <p>
                以下为您所选的复制数据，请在输入框内按CTRL+A或右键选择复制，然后粘贴到QQ等处</p>
            <textarea class="text textarea_width" rows="4" cols="10" id="content"></textarea>
        </div>
        <div class="btns">
            <button class="btn class1 close" type="button">
                关&nbsp;&nbsp;闭</button>
        </div>
    </div>
    <asp:HiddenField ID="hfdTransferIds" runat="server" />
    </form>
</body>
</html>
<script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../Scripts/widget/common.js" type="text/javascript"></script>
<script src="../../Scripts/Global.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(":checkbox", "table tr td").click(function () {
            if (parseFloat($(":checkbox", "table tr td").length) == parseFloat($(":checkbox:checked", "table tr td").length)) {
                $("#check_All").attr("checked", "checked");
            } else {
                $("#check_All").removeAttr("checked");
            }
        })
        $("#btnSubmit").click(function () {
            if ($("#radioMessage").is(":checked")) {
                if ($.trim($("#txtMessage").val()).length == 0) {
                    alert("请输入短信内容");
                    return false;
                }
                if ($.trim($("#txtMessage").val()).length > 300) {
                    alert("短信字数不能超过300位");
                    return false;
                }
            }
            if ($("#radioHandler").is(":checked")) {
                if ($.trim($("#txtNoticeRemark").val()).length > 1000) {
                    alert("通知备注不能超过1000位");
                    return false;
                }
            }
        });
        $("#copyPnr").click(function () {
            if (showCheckbox('PNR')) {
                var str = '';
                for (var i = 0; i < $(":checkbox:checked", "table tr td").length; i++) {
                    str += $(":checkbox:checked", "table tr td").eq(i).attr("pnr") + ',';
                }
                str = str.substring(0, str.length - 1);
                if ($.browser.msie) {
                    copyToClipboard(str);
                } else {
                    $("#content").text(str);
                    $("#divOption").click();
                }
            }
        });
        $("#copyOrderId").click(function () {
            if (showCheckbox('订单号')) {
                var str = '';
                for (var i = 0; i < $(":checkbox:checked", "table tr td").length ; i++) {
                    str += $(":checkbox:checked", "table tr td").eq(i).attr("orderId") + ',';
                }
                str = str.substring(0, str.length - 1);
                if ($.browser.msie) {
                    copyToClipboard(str);
                } else {
                    $("#content").text(str);
                    $("#divOption").click();
                }
            }
        });
        $("#btnNoticeHandler").click(function () {
            var str = '';
            for (var i = 0; i < $(":checkbox:checked", "table tr td").length; i++) {
                str += $(":checkbox:checked", "table tr td").eq(i).attr("id") + ',';
            }
            str = str.substring(0, str.length - 1);
            if (str.length == 0) {
                alert("请先选择需要被通知的数据行");
            } else {
                $("#hfdTransferIds").val(str);
                if ($("#radioMessage").is(":checked")) {
                    $("#divMessage").show();
                    $("#divPersonHandler").hide();
                } else {
                    $("#divMessage").hide();
                    $("#divPersonHandler").show();
                }
                $("#divOpcial").click();
            }
        });
        $("#radioHandler").click(function () {
            $("#divMessage").hide();
            $("#divPersonHandler").show();
        });
        $("#radioMessage").click(function () {
            $("#divMessage").show();
            $("#divPersonHandler").hide();
        });
        $("#txtMessage").keyup(function () {
            $("#messageCount").html($.trim($(this).val()).length);
        });
    })
    function checkAll(all) {
        var checks = document.getElementsByTagName("input");
        for (var i = 0; i < checks.length; i++) {
            if (checks[i].type == "checkbox" && checks[i].id != "check_All") {
                checks[i].checked = all.checked;
            }
        }
    }
    function showCheckbox(content) {
        var a = $(":checkbox:checked");
        if (a.length < 1) { alert("请先选择需要被复制" + content + "的数据行！"); return false; }
        return true;
    }
</script>
