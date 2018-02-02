<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coordination.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Operate.Coordination" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link rel="stylesheet" href="/styles/coordinate.css" />

<body>
    <%--错误信息--%>
    <div runat="server" id="divError" class="column hd">
    </div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">
        业务协调</h3>
    <div class="form column">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <!--订单-->
            <tbody runat="server" id="order">
                <tr>
                    <td class="title">
                        订单编号
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOrderId" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        订单状态
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOrderStatus" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        产品类型
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        创建时间
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProducedTime" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        支付时间
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblPayTime" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
            </tbody>
            <!--申请单-->
            <tbody runat="server" id="applyform">
                <tr>
                    <td class="title">
                        申请单号
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        申请单状态
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblApplyformStatus" CssClass="obvious"></asp:Label>
                    </td>
                    <td class="title">
                        产品类型
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbllProductType" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        申请时间
                    </td>
                    <td colspan="5">
                        <asp:Label runat="server" ID="lblAppliedTime" CssClass="obvious"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="form" runat="server" id="divOemInfo">
        <h3 class="titleBg">
            OEM信息</h3>
        <table>
            <tr>
                <td class="title">
                    用户名：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemCompanyUserName"></asp:Literal>
                </td>
                <td class="title">
                    公司类型：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyTypeValue" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    授权时间：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemAuthorizationTime"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司名称：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemCompayName"></asp:Literal>
                </td>
                <td class="title">
                    简称：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemCompanyAbbreviation"></asp:Literal>
                </td>
                <td class="title">
                    OEM名称：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemName"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司电话：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemCompayPhone"></asp:Literal>
                </td>
                <td class="title">
                    组织机构代码证：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemCompanyOrginationCode"></asp:Literal>
                </td>
                <td class="title">
                    授权域名：
                </td>
                <td>
                    <asp:Literal runat="server" ID="lblOemDomainName"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="title">
                    负责人：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyManagerName" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    手机：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyManagerCellPhone" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    授权状态：
                </td>
                <td>
                    <asp:Literal ID="lblOemValid" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyLocation" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    地址：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyAddress" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    授权到期：
                </td>
                <td>
                    <asp:Literal ID="lblOemEffectTime" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="title">
                    附件：
                </td>
                <td>
                    <a href="#" runat="server" id="lbtnBussinessLicense" target="_blank">营业执照</a>&nbsp;&nbsp;
                    <a href="#" runat="server" id="lbtncertNo" target="_blank" visible="false">身份证</a>&nbsp;&nbsp;
                    <a href="#" runat="server" id="lbtnIATA" target="_blank">IATA</a>
                </td>
                <td class="title">
                    使用期限：
                </td>
                <td>
                    <asp:Literal ID="lblOemEffectTimeStrat" runat="server"></asp:Literal>
                    <asp:Literal ID="lblOemEffectTimeEnd" runat="server"></asp:Literal>
                </td>
                <td class="title">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="title">
                    开户时间：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyRegisterTime" runat="server"></asp:Literal>
                </td>
                <td class="title">
                    审核时间：
                </td>
                <td>
                    <asp:Literal ID="lblOemCompanyAuditTime" runat="server"></asp:Literal>
                </td>
                <td class="title">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div class="form  volumn drop-box">
       <%-- <h3 class="titleBg">
            采购方</h3>--%>
        <div class="clearfix">
            <h4 class="drop-header" id="onDropHeader">
                展开采购信息</h4>
        </div>
        <div class="drop-content hidden">
            <p class="type">采购方</p>
            <table>
                <tr>
                    <td class="title">
                        公司名称：
                    </td>
                    <td>
                        <a runat="server" id="lnkPurchaserCompanyName"></a>
                    </td>
                    <td class="title">
                        联系人：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserContact"></asp:Label>
                    </td>
                    <td class="title">
                        类型：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserCompanyType"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        公司电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserCompanyTel"></asp:Label>
                    </td>
                    <td class="title">
                        联系电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserContactPhone"></asp:Label>
                    </td>
                    <td class="title">
                        紧急联系电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserEmergentContactPhone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        所在地：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserLocation"></asp:Label>
                    </td>
                    <td class="title">
                        公司地址：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserAddress"></asp:Label>
                    </td>
                    <td class="title">
                        邮编：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPurchaserPostcode"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            $(function () {
                if ($("#divOemInfo").length > 0) {
                    $("#onDropHeader").toggle(function () {
                        $(this).html("收起采购信息").parent().next().slideToggle("slow");
                    }, function () {
                        $(this).html("展开采购信息").parent().next().slideToggle("slow");
                    });
                } else {
                    $("#onDropHeader").parent().addClass("hidden").next().removeClass("hidden").before('<h3 class="titleBg">采购方</h3>').find("p").addClass("hidden");
                }
            });
        </script>
    </div>
    <div class="form">
        <h3 class="titleBg">
            订单联系信息</h3>
        <table>
            <tr>
                <td class="title">
                    姓名：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOrderContact"></asp:Label>
                </td>
                <td class="title">
                    手机号码：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOrderContactMobile"></asp:Label>
                </td>
                <td class="title">
                    电子邮箱：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOrderContactEmail"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <!-- 乘机人联系信息 -->
    <div class="column table">
        <h3 class="titleBg">
            乘机人联系信息</h3>
        <asp:Repeater runat="server" ID="passengers">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>
                            姓名
                        </th>
                        <th>
                            证件号
                        </th>
                        <th>
                            联系电话
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("Name")%>
                    </td>
                    <td>
                        <%# Eval("Credentials")%>
                    </td>
                    <td>
                        <%# Eval("Phone")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="form" runat="server" id="divSupplier" visible="False">
        <h3 class="titleBg">
            资源方</h3>
        <table>
            <tr>
                <td class="title">
                    公司名称：
                </td>
                <td>
                    <a runat="server" id="lnkSupplierCompanyName"></a>
                </td>
                <td class="title">
                    联系人：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblSupplierContact"></asp:Label>
                </td>
                <td class="title">
                    联系电话：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblSupplierContactPhone"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="form" runat="server" id="divProvider">
        <h3 class="titleBg">
            出票方</h3>
        <table>
            <tbody>
                <tr>
                    <td class="title">
                        公司名称：
                    </td>
                    <td>
                        <a runat="server" id="lnkProviderCompanyName"></a>
                    </td>
                    <td class="title">
                        联系人：
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblProviderContact"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        公司电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderCompanyTel"></asp:Label>
                    </td>
                    <td class="title">
                        联系电话：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderContactPhone"></asp:Label>
                    </td>
                    <td class="title">
                        传真：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderFax"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        所在地：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderLocation"></asp:Label>
                    </td>
                    <td class="title">
                        公司地址：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderAddress"></asp:Label>
                    </td>
                    <td class="title">
                        邮编：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderPostcode"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        工作时间：
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblProviderWorkingTime"></asp:Label>
                    </td>
                    <td class="title">
                        废票时间：
                    </td>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblProviderScrapTime"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="column table" runat="server" id="divProviderBusinessInfo">
        <h3 class="titleBg">
            供应方业务联系信息</h3>
        <asp:Repeater runat="server" ID="businessManagers">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>
                            岗位
                        </th>
                        <th>
                            联系人
                        </th>
                        <th>
                            联系电话
                        </th>
                        <th>
                            QQ/MSN
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("BusinessName")%>
                    </td>
                    <td>
                        <%# Eval("Mamanger")%>
                    </td>
                    <td>
                        <%# Eval("Cellphone")%>
                    </td>
                    <td>
                        <%# Eval("QQ")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div runat="server" id="divCoordination" class="column table">
        <h3 class="titleBg">
            订单标识信息</h3>
        <div>
            <table>
                <tr>
                    <th class="w15">
                        时间
                    </th>
                    <th>
                        原因
                    </th>
                    <th class="w15">
                        操作人账号
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblIdentificationTime"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblIdentificationContent"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblIdentificationAccount"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="coordinate" style="margin-top: 10px;">
            <h3>
                协调信息</h3>
            <asp:Literal ID="divCoordinationContent" runat="server" />
            <div class="c_handle">
                <asp:TextBox runat="server" ID="txtCoordinationContent" TextMode="MultiLine" Columns="100"
                    Visible="false" Style="width: 99.9%; height: 100px;" CssClass="text radius"></asp:TextBox>
            </div>
            <div class="c_handle">
                <asp:DropDownList runat="server" ID="ddlContactMode" Style="width: 15%;" class="radius">
                </asp:DropDownList>
                <asp:DropDownList ID="dropIdentificationOrder" Enabled="false" runat="server" Style="width: 20%;"
                    class="radius">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlCoordinationContent" runat="server" Style="width: 60%;"
                    class="radius">
                </asp:DropDownList>
            </div>
            <div class="c_handle">
                协调结果：
                <asp:TextBox runat="server" ID="txtCoordinationResult" Style="width: 80%;" class="text radius"></asp:TextBox>
                <asp:DropDownList ID="ddlCoordinationResult" runat="server" Style="width: 10%;" class="radius">
                </asp:DropDownList>
            </div>
            <asp:HiddenField runat="server" ID="hidBusinessType" />
        </div>
        <%--<div>
            <div class="column">
                
                联系方式：
                <span class="obvious1 pad-l">您可以通过电话、短信、QQ、邮件或其他方式进行协调</span>
            </div>
            <div class="column">
                协调内容：
                
                <br />
                <br />
                <div>
                    
                </div>
            </div>
            <div class="column">
                协调结果：
                
                <br />
                <br />
                <div>
                    
                </div>
            </div>
            <div class="column" runat="server" id="divIdentificationOrder" visible="false">
                订单标识：
                
                <br />
                <br />
                <div>
                    <asp:TextBox runat="server" ID="txtIdentificationOrder" TextMode="MultiLine" Rows="5"
                        Columns="100" Style="width: 620px; border-color: #aaa; padding: 5px;"></asp:TextBox>
                </div>
            </div>
        </div>--%>
        <div class="column btns">
            <asp:Button runat="server" ID="btnSave" CssClass="btn class1" Text="保&nbsp;&nbsp;&nbsp;存"
                OnClientClick="return checkCoordination();" OnClick="btnSave_Click" />
            <asp:Button runat="server" ID="btnSaveAndBack" CssClass="btn class1" Text="保存并返回"
                OnClientClick="return checkCoordination();" OnClick="btnSaveAndBack_Click" />
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidReturnUrl" />
    <asp:HiddenField runat="server" ID="hidOrderId" />
    <asp:HiddenField runat="server" ID="hidApplyformId" />
    <asp:HiddenField runat="server" ID="hidOrderStatus" />
    </form>
    <div class="column btns">
        <button class="btn class1" runat="server" id="btnDetail" visible="false">
            详&nbsp;&nbsp;&nbsp;情</button>
        <button class="btn class2" runat="server" id="btnBack">
            返&nbsp;&nbsp;&nbsp;回</button>
    </div>
</body>
</html>
<script type="text/javascript" language="javascript">
    $(function () {
        $("#ddlCoordinationContent").change(function () {
            $("#txtCoordinationContent").val($(this).val());
        });
        $("#ddlCoordinationResult").change(function () {
            $("#txtCoordinationResult").val($(this).val());
        });
        $("#dropIdentificationOrder").change(function () {
            $("#txtIdentificationOrder").val($(this).val());
        });
    });
    function checkCoordination() {
        var content = $.trim($("#txtCoordinationContent").val());
        var result = $.trim($("#txtCoordinationResult").val());
        var identity = $.trim($("#txtIdentificationOrder").val());
        if ($("#divIdentificationOrder").size() <= 0) {
            if ($("#ddlContactMode").val() == '') {
                alert("请选择联系方式");
                $("#ddlContactMode").focus();
                return false;
            }
            if (content == '') {
                alert("请输入协调内容");
                $("#txtCoordinationContent").select();
                return false;
            } else if (content.length > 200) {
                alert("协调内容不能超过200字");
                $("#txtCoordinationContent").select();
                return false;
            }
            if (result == '') {
                alert("请输入协调结果");
                $("#txtCoordinationResult").select();
                return false;
            } else if (result.length > 200) {
                alert("协调结果不能超过200字");
                $("#txtCoordinationResult").select();
                return false;
            }
            return true;
        } else {
            if (content != "" && content.length > 200) {
                alert("协调内容不能超过200字");
                return false;
            }
            if (result != "" && result.length > 200) {
                alert("协调结果不能超过200字");
                return false;
            }
            if (identity != "" && identity.length > 200) {
                alert("订单标识内容不能超过200字");
                return false;
            }
            return true;
        }
    }
</script>
