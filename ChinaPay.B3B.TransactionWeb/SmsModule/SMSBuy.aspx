<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSBuy.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SmsModule.SMSBuy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>购买短信套餐</title>
</head>
    <link href="/Styles/icon/fontello.css" rel="stylesheet" />
    <link href="/Styles/bank.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style>
        .font-b
        {
            font-size: 16px;
            font-weight: 600;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        购买短信套餐</h3>
    <div class="box-a">
        尊敬的用户您好，您当前共有积分：<asp:Label ID="lblZong" runat="server" CssClass="obvious" />
        分&nbsp;&nbsp;&nbsp;可用积分：<asp:Label ID="lblKeYong" runat="server" CssClass="obvious" />
        分 <a href="/About/help.aspx?flag=jifenhuode" target="_blank" class="obvious-a">如何获得积分？</a>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <a class="obvious-a" href="/IntegralCommodity/IntegralZengZhang.aspx">积分增长记录</a>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <a class="obvious-a" href="/IntegralCommodity/IntegralXiaoFei.aspx">积分消费记录</a>
    </div>
    <div class="table">
        <table class="noTopBorder">
            <tr>
                <th>
                    金额（元）
                </th>
                <th>
                    可发送条数
                </th>
                <th>
                    单位（元/条）
                </th>
                <th>
                    备注
                </th>
                <th style="width: 190px;">
                    购买数量（份）
                </th>
                <th>
                    在线购买
                </th>
            </tr>
            <%
                
                var query = ChinaPay.SMS.Service.SMSProductService.QueryAllValid();
                foreach (var item in query)
                {
            %>
            <tr>
                <td class="font-b">
                    <%=item.Amount.ToString("C")%>
                </td>
                <td>
                    <%=item.Count%>
                </td>
                <td>
                    <%=item.UnitPrice%>
                </td>
                <td>
                    <%=item.ExChangeIntegral <= 0 ? "暂不支持积分兑换" : "可用 "+item.ExChangeIntegral+" 积分兑换"%>
                </td>
                <td style="width: 300px;">
                    <% if (item.ExChangeIntegral <= 0)
                       {
                    %>
                    <select class="text fl selexchange" style="width: 100px;">
                        <option value="0">现金购买</option>
                    </select>
                    <%
                        }
                       else
                       {
                    %>
                    <select class="text fl selexchange" style="width: 100px;" exchangenumber="<%=item.ExChangeIntegral %>">
                        <option value="0">现金购买</option>
                        <option value="1">积分兑换</option>
                    </select>
                    <%
                        }
                    %><div class="addnum-box fl" count="<%=item.Count%>">
                        <input type="text" value="1" maxlength="4" class="txtCount" />
                        <a href="javascript:void(0);" class="reduce">-</a> <a href="javascript:void(0);"
                            class="add">+</a>
                    </div>
                    <div class="fl">
                        份 共<span class="obvious"><%=item.Count%></span>条</div>
                </td>
                <td>
                    <input type="button" class="btn class1" value="在线购买" onclick="buySms('<%=item.Amount %>','<%=item.Count %>','<%=item.Id %>',this);" />
                </td>
            </tr>
            <%
}
            %>
        </table>
        <div class="importantBox broaden">
            <p class="important">
                短信只针对机票类型信息发送，禁止发送违禁语言（如广告信息等），否则将禁用您的短信功能，敬请知悉。</p>
        </div>
        <a id="layer4" style="display: none;" data="{type:'pop',id:'divlayer'}"></a>
        <div class="layer4" id="divlayer" style="display: none;">
            <h4>
                操作提示：<a href="javascript:;" class="close">关闭</a></h4>
            <p class="layerTips">
                抱歉,您的可用积分不足以支付该短信套餐,您可以使用现金方式进行购买<br />
                您也可以通过每日登录或购买机票的方式加油积攒积分再来兑换<br />
                <span class="obvious" style="font-weight: bold;">所需积分：<label id="lblsxjf"></label>分</span>,<span
                    style="font-weight: bold;">您可用积分：<label id="lblkyjf"></label></span>,还差<label id="lblhcjf"></label>分
            </p>
            <div class="layerBtns text-c">
                <a href="javascript:;" class="btn class1" id="btnCash">使用现金购买</a> <a href="javascript:;"
                    class="btn class2 close">关闭</a>
            </div>
        </div>
        <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
        <div id="divPolicy" class="form layer" style="display: none;">
            <h4>
                操作提示：</h4>
            <table>
                <tr>
                    <td class="title">
                        <span class="fl">套餐名称:<label id="lbltcmc" runat="server"></label></span>
                    </td>
                    <td class="title">
                        <span class="fl">购买数量:<label id="lblgmsl"></label></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span class="fl obvious" style="font-weight: bold;">所需积分:<label id="lblsxjft"></label></span>
                    </td>
                    <td class="title">
                        <span class="fl">您当前可用积分:<label id="lbldqkyjf"></label></span>
                    </td>
                </tr>
                <tr class="btns">
                    <td colspan="2">
                        <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="确认并兑换" OnClick="btnSave_Click" />
                        <input class="close btn class2" value="关&nbsp;&nbsp;&nbsp;闭" type="button" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button runat="server" ID="btnBuy" OnClick="btnBuy_Click" CssClass="hide" />
        <asp:HiddenField ID="hidbackPageId" runat="server" />
        <asp:HiddenField ID="hidsmsNumber" runat="server" />
        <asp:HiddenField ID="hidexchange" runat="server" />
        <asp:HiddenField ID="hidjfNum" runat="server" />
        <asp:HiddenField ID="hidjf" runat="server" />
        <asp:HiddenField ID="hidShow" runat="server" />
        <asp:HiddenField ID="hidName" runat="server" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js?20121101" type="text/javascript"></script>
<script type="text/javascript">
    function buySms(price, count, id, para) {
        $("#hidbackPageId").val(id);
        $("#hidsmsNumber").val($(para).parent().parent().find(".txtCount").val());
        $("#hidexchange").val($(para).parent().parent().find(".selexchange").val());
        if ($("#hidexchange").val() == "1") {
            var ex = $(para).parent().parent().find(".selexchange").attr("exchangenumber");
            var jf = (parseInt(ex) * parseInt($("#hidsmsNumber").val()));
            $("#hidjf").val(ex); $("#hidjfNum").val(count);
            var kyjf = $("#lblKeYong").html();
            if (parseInt(kyjf) < parseInt(jf)) {
                $("#layer4").click();
                $("#lblsxjf").html(jf);
                $("#lblkyjf").html(kyjf);
                $("#lblhcjf").html(parseInt(jf) - parseInt(kyjf));
            } else {
                $("#divOpcial").click();
                $("#lbltcmc").html(parseInt(price) + "元短信套餐（可" + count + "发条）");
                $("#hidName").val($("#lbltcmc").html());
                $("#lblgmsl").html($("#hidsmsNumber").val() + "份");
                $("#lblsxjft").html("<span class='obvious'>" + jf + "分</span><span class='obvious1'>(" + ex + "分/份)</span>");
                $("#lbldqkyjf").html(kyjf + "分");
            }
        } else if ($("#hidexchange").val() == "0") {
            $("#btnBuy").click();
        }
    }
    $(function () {
        //        if ($("#hidShow").val()=="1") {
        //            $("#divOpcial").click();
        //        }
        $("#btnCash").click(function () {
            $("#hidexchange").val("0");
            $("#btnBuy").click();
        });
        $(".reduce").click(function () {
            var count = parseInt($(this).parent().find(".txtCount").val());
            if (count <= 1) {
                $(this).parent().find(".txtCount").val("1");
            } else {
                $(this).parent().find(".txtCount").val(count - 1);
            }
            $(this).parent().parent().find(".obvious").html(parseInt(parseInt($(this).parent().find(".txtCount").val()) * parseInt($(this).parent().attr("count"))));

        });
        $(".add").click(function () {
            var count = parseInt($(this).parent().find(".txtCount").val());
            if (count >= 9999) {
                $(this).parent().find(".txtCount").val("9999");
            } else {
                $(this).parent().find(".txtCount").val(count + 1);
            }
            $(this).parent().parent().find(".obvious").html(parseInt(parseInt($(this).parent().find(".txtCount").val()) * parseInt($(this).parent().attr("count"))));
        });
        $(".txtCount").keyup(function () {
            var value = $(this).val();
            var reg = /^[0-9]{1,4}?$/;
            if (!reg.test(value)) {
                $(this).val("1");
            }
            $(this).parent().parent().find(".obvious").html(parseInt(parseInt($(this).val()) * parseInt($(this).parent().attr("count"))));
        });
    });
</script>
