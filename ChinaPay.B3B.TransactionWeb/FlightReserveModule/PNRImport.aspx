<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PNRImport.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.PNRImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="/Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" class="rightContent">
    <ul class="importNav clearfix">
        <li><a href="javascript:void(0);" id="pnrImport">编码导入</a> </li>
        <li><a href="javascript:void(0);" class="curr" id="contentImport">内容导入</a> </li>
    </ul>
    <div class="importBox" id="divPNRImport">
        <p class="importTips">
            建议使用PNR编码导入的类型：<span>普通单程</span><span>特价单程</span></p>
        <div class="PNRBox">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:RadioButton runat="server" ID="radAdultPNR" Checked="True" Text="成人编码" GroupName="radPNRType" />
                        <asp:RadioButton runat="server" ID="radChildrenPNR" Text="儿童编码" GroupName="radPNRType" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 60px;">
                        <div id="divAdultPNRContent">
                            <span>输入成人编码：</span>
                            <asp:TextBox runat="server" ID="txtPNRCode" class="text" />
                        </div>
                        <div id="divChildrenPNRContent" style="display: none;">
                            <span>输入儿童编码：</span>
                            <asp:TextBox runat="server" ID="txtChildrenPNRCode" class="text" />
                            <br />
                            <span>备注成人编码：</span>
                            <asp:TextBox runat="server" ID="txtAdultPNRCode" class="text" />
                        </div>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnPNRCodeImport" CssClass="btn class1" Text="提交PNR编码"
                            OnClick="btnPNRCodeImport_Click" OnClientClick="return AddPNRPat()" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="PNRTips">
            <h4>
                PNR导入注意事项：</h4>
            <p>
                <span class="numList">1、</span>重要更新提示：国航（CA）、海航（HU）、联航（KN）编码需要加OSI项</p>
            <p>
                指令：OSI航空公司代码CTCT您的手机号码/Pn<span class="eg">例如：OSI CA CTCT138*****888/P1</span></p>
            <p>
                <span class="numList">2、</span>该PNR姓名组正确，航段信息正确、舱位状态正确，每个乘客均有真实的SSR FOID项输入</p>
            <p>
                <span class="numList">3、</span>该PNR不能包含票价组项，如FC/FN/FP，儿童编码导入时，需要备注成人编码</p>
        </div>
    </div>
    <div class="importBox" id="divContentImport">
        <p class="importTips">
            建议使用PNR内容导入的类型：<span>普通往返</span><span>普通联程</span><span>特价往返</span><span>特价联程</span><span>团队票</span><span>缺口程</span></p>
        <div class="PATBox clearfix">
            <div class="fl">
                <h4>
                    以下为示例内容：</h4>
                <div>
                    <textarea class="eg" readonly="readonly">
>RTHTDHLV
 1.王大明 HTDHLV
 2.  HO1322 M   SU13NOV  KWLSHA HK1   1240 1535          E --T2
 3.PEK/T PEK/T99889966/HUOXINQIULAIDE SOUTHWEST TRAVEL/HUO XIN QIU ABCDEFG
 4.13800138000/P1
 5.TL/1200/13NOV/YYY111
 6.SSR FOID HO HK1 NI532131199000322071/P1
 7.SSR ADTK 1E BY YYY11NOV11/1240 OR CXL HO1322 M13NOV 
 8.OSI HO CTCT13888138088/P1
 9.RMK CA/HYGNKR
10.YYY222
>
>PAT:A
PAT:A 
01 M FARE:CNY1520.00 TAX:CNY50.00 YQ:CNY130.00 TOTAL:1700.00 
>SFC:01 
>
</textarea>
                </div>
            </div>
            <div class="fr">
                <p>
                    <asp:RadioButton runat="server" ID="radAdultPNRContent" GroupName="radPassengerType"
                        Text="成人编码" Checked="True" />
                    <asp:RadioButton runat="server" ID="radChildrenPNRContent" GroupName="radPassengerType"
                        Text="儿童编码" />
                    <span id="AdultPNRCodeForContentTitle">备注成人编码：</span>
                    <asp:TextBox runat="server" ID="txtAdultPNRCodeForContent" CssClass="text"></asp:TextBox>
                </p>
                <asp:TextBox runat="server" Rows="2" Columns="20" CssClass="pnrContent" ID="txtPNRContent"
                    TextMode="MultiLine"></asp:TextBox>
                <div class="btns">
                    <asp:Button runat="server" ID="btnPNRContentImport" CssClass="btn class1" Text="立即提交"
                        OnClick="btnPNRContentImport_Click" OnClientClick="return CheckContent()"/>
                </div>
            </div>
        </div>
    </div>
    <a id="divOpcial" style="display: none" data="{type:'pop',id:'div_PATContent'}"></a>
    <div class="layer4 importLayer" id="div_PATContent" style="display: none">
        <h4>
            操作提示：<a href="javascript:void(0);" class="close">关闭</a></h4>
        <p class="important">
            请输入PAT信息</p>
        <div class="addBox">
            <p>
                请在此处粘贴PAT内容</p>
            <textarea rows="2" cols="20" id="txtPATContent" runat="server"></textarea>
            <div class="layerBtns">
                <%--<asp:Button ID="btnPATSubmit" Text="提交" runat="server" CssClass="btn class1" />--%>
                <input type="button" id="btnPATSubmit" value="提交" class="btn class1"/>
                <a href="javascript:void(0);" class="btn class2 close">取消</a>
            </div>
        </div>
        <div class="tipsBox">
            <p>
                操作帮助：在RT编码后，“成人编码”请您再执行“PAT:A”指令，如：</p>
            <p>
                &gt;<b class="b">PAT:A</b></p>
            <p>
                PAT:A</p>
            <p>
                01 M FARE:CNY1520.00 TAX:CNY50.00 YQ:CNY130.00 TOTAL:1700.00</p>
            <p>
                &gt;>SFC:01 </p>
            <p>&gt;</p>
            <p>
                操作帮助：在RT编码后，“儿童编码”请您在执行“PAT:A*CH”指令，如：</p>
            <p>
                &gt;<b class="b">PAT:A*CH</b></p>
            <p>
                PAT:A</p>
            <p>
                01 M FARE:CNY1520.00 TAX:CNY50.00 YQ:CNY130.00 TOTAL:1700.00 </p>
            <p>
                &gt;SFC:01</p>
            <p>&gt;</p>
            <p>
                最后将PAT指令所获得的内容复制到PAT内容输入框中，点击确认生成订单</p>
        </div>
    </div>
    <a id="divWarnInfo" style="display: none" data="{type:'pop',id:'div_WarnInfo'}"></a>
    <div class="layer4 importLayer" id="div_WarnInfo" style="display: none">
        <h4>
            操作提示：<a href="javascript:void(0)" class="close">关闭</a></h4>
        <p class="important">
        <label id="lblWarnInfo">请输入正确的PNR内容</label>
            </p>
        <div class="layerBtns">
            <a href="javascript:void(0);" class="btn class1 close">确定</a>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="sate" Value="1"/>
    <asp:HiddenField runat="server" ID="hfdServicePhone"/>
    <asp:HiddenField runat="server" ID="hfdEnterpriseQQ" />
    </form>
</body>
</html>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/FlightModule/pnrImport.js?20130424" type="text/javascript"></script>
<script type="text/javascript">
    var origialContent = "温馨提示：\n1.请用您的黑屏系统，调取PNR内容，再执行PAT-A指令，将其全部内容复制，并粘贴到黑色输入框内；\n2.您需要注意的是：复制的内容必须从RT编码开始复制到PAT:A结果的所有内容；\n3.点击查看演示说明；\n4.如有不会使用请拨打帮助热线：" + $("#hfdServicePhone").val() + "或加企业QQ:" + $("#hfdEnterpriseQQ").val() + "";
    var patContent;
    var patReg = /<%=PatReg %>/;
    var hasPat = true;
    $(function ()
    {
        if ($("#txtPNRContent").val() == "") $("#txtPNRContent").val(origialContent);
        if ($("#radChildrenPNRContent").is(":checked"))
        {//若选择了儿童编码，则重新显示成人编码备注输入框
            $("#txtAdultPNRCodeForContent").show();
        }
        if ($("#radChildrenPNR").is(":checked")) {
            $("#divAdultPNRContent").hide();
            $("#divChildrenPNRContent").show();
        }

        $("#divPNRImport").hide();
        $("#divContentImport").show();
        $("#pnrImport").click(function ()
        {
            $("#sate").val("0");
            $(this).addClass("curr");
            $("#contentImport").removeClass("curr");
            $("#divPNRImport").show();
            $("#divContentImport").hide();
        });
        $("#contentImport").click(function ()
        {
            $("#sate").val("1");
            $(this).addClass("curr");
            $("#pnrImport").removeClass("curr");
            $("#divPNRImport").hide();
            $("#divContentImport").show();
        });
        if ($("#sate").val() == "0")
            $("#pnrImport").trigger("click");
        var contentInputer = $("#txtPNRContent");
        //origialContent = contentInputer.val();
        contentInputer.focus(function ()
        {
            if ($(this).val() == origialContent)
            {
                $(this).val("");
            }
        }).blur(function ()
        {
            if ($.trim($(this).val()) == "")
            {
                $(this).val(origialContent);
            }
        });

        //输入完成PAT时追加内容并提交
        $("#btnPATSubmit").click(function ()
        {
            var contentCtl = $("#txtPATContent");
            var pnrContentCtl = $("#txtPNRContent");
            if ($.trim(contentCtl.val()) == "")
            {
                alert("请输入PAT内容");
                return false;
            }
            if (!patReg.test(contentCtl.val()))
            {
                alert("PAT内容格式不正确");
                return false;
            }
            hasPat = true;
            patContent = contentCtl.val();
            $(".close").click();
            if ($("#divPNRImport").is(":visible")) $("#btnPNRCodeImport").trigger("click");
            else
            {
                pnrContentCtl.val(pnrContentCtl.val() + "\n" + contentCtl.val());
                $("#btnPNRContentImport").trigger("click");
            }
        });
    });

    function CheckContent()
    {
        var isMutiFlight = false;  //TODO   识别出单程航班，将此值设置成false,跳过PAT内容输入,暂定是所有都需要PAT
        var contentCtl = $("#txtPNRContent");
        if (contentCtl.val() == "") {
            alert("请输入PNR内容");
            return false;
        }
        if(isMutiFlight&&!patReg.test(contentCtl.val()))
        {
            $("#divOpcial").click();    
            return false;
        }
        return true;
    }
    function AddPNRPat()
    {
        if ($("#radAdultPNR").attr('checked'))
        {
            if (!validatePNRCode("txtPNRCode", "成人编码")) return false;
        } else
        {
            if(!validatePNRCode("txtAdultPNRCode", "成人编码") && !validatePNRCode("txtChildrenPNRCode", "儿童编码")) return  false;
        }
        if (hasPat) return true;
        $("#divOpcial").click();
        return false;
    }


</script>
