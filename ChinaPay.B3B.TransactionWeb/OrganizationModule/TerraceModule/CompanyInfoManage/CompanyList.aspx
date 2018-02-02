<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.CompanyList"
    EnableEventValidation="false" %>

<%@ Import Namespace="ChinaPay.B3B.Common.Enums" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公司信息管理</title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maxWidth {
            max-width: 80px;
            word-break: break-all;
            word-wrap: break-word;   
        }
        td.controls {
            width: 260px;
            padding-left: 30px;
        }
        td.controls div {
            width: 60px;
            float: left;   
        }
        #dataListTable td {
            vertical-align: middle;   
        }.dlLeft
        {
            float:left;
        }
        .warn
        {
            color:Red;
        }        
    </style>
<body>
    <form id="form1" defaultbutton="btnQuery" runat="server">
    <div class="hd">
        <h3 class="titleBg">
            公司管理</h3>
    </div>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox id="txtAbbreviateName" class="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input up-index" style="z-index: 53">
                            <span class="name">公司类别：</span>
                            <div class="dlLeft"><asp:DropDownList runat="server" ID="ddlCompanyType" AppendDataBoundItems="true" Width="78px">
                                <asp:ListItem Value="">全部</asp:ListItem>
                            </asp:DropDownList></div>
                            <div class="dlLeft"><asp:DropDownList runat="server" ID="ddlAccountType" Width="65px">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:listitem text="个人" Value="0" />
                                <asp:listitem text="企业" Value="1" />
                            </asp:DropDownList></div>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">联系人：</span><asp:TextBox id="txtContact" class="text" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">用户名：</span>
                            <asp:TextBox id="txtAccount" class="text" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="input up-index" style="z-index: 52">
                            <span class="name">状态：</span>
                            <asp:DropDownList runat="server" ID="ddlStatus">
                                <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="1">正常</asp:ListItem>
                                <asp:ListItem Value="0">禁用</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                       <div class="input">
                         <span class="name">审核状态：</span>
                         <asp:DropDownList ID="ddlCompanyAuditStatus" runat="server">
                         </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr class="btns">
                 <td colspan="3">
                    <asp:Button ID="btnQuery" class="btn class1" Text="查询" runat="server" onclick="btnQuery_Click" OnClientClick="SaveSearchCondition('CompanyList')" />
                        <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                 </td>
                </tr>
                <%--<tr>
                    <td>    
                        <div class="input up-index">
                            <span class="name">所在省份：</span>
                            <asp:DropDownList runat="server" ID="ddlProvince">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="input up-index">
                            <span class="name">所在城市：</span>
                            <select id="ddlCity">
                            </select>
                            <asp:HiddenField runat="server" ID="hidCity" />
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">注册日期：</span>
                             <asp:TextBox id="txtRegisterDateLower" class="text text-s" type="text" runat="server"
                                onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'#F{$dp.$D(\'txtRegisterDateUpper\')}'})" />
                            <asp:TextBox id="txtRegisterDateUpper" class="text text-s" type="text" runat="server"
                                onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d',minDate:'#F{$dp.$D(\'txtRegisterDateLower\')}'})" />
                        </div>
                        <div class="input">
                            <span class="name">审核时间：</span>
                            <asp:TextBox id="txtAuditDateLower" class="text text-s" type="text" runat="server"
                                onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'%y-%M-%d'})" />
                            <asp:TextBox id="txtAuditDateUpper" class="text text-s" type="text" runat="server"
                                onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'%y-%M-%d'})" /><asp:HiddenField runat="server" ID="hdDefaultDate"/>
                        </div>
                    </td>
                </tr>--%>
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table id="dataListTable">
                <colgroup>
                    <col />
                    <col span="5" class="w10"/>
                    <col />
                </colgroup>
                <thead>
                <tr>
                    <th>用户名</th>
                    <th>公司类别</th>
                    <th>公司简称</th>
                    <th>联系人</th>
                    <%-- <th>所在地</th>
                   <th>联系电话</th>
                    <th>上次登录时间</th>
                    <th>注册时间</th>
                    <th>审核时间</th>
                    <th>使用期限</th>
                    --%>
                    <th>审核状态</th>
                    <th>状态</th>
                    <th>注册时间<br />上次登录时间</th>
                    <th>操作</th>
                </tr>
                </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
		        <td><a href="LookUpCompanyInfo.aspx?CompanyId=<%#Eval("CompanyId") %>&SerachCondition=<%#Eval("SerachCondition") %>"><%#Eval("UserNo")%></a></td>
		        <td><%#Eval("CompanyTypeText")%> (<%#Eval("AccountType")%>)</td>
		        <td><%#Eval("AbbreviateName")%></td>
		        <td><%#Eval("Contact")%></td>
		        <%--<td><%#Eval("Address")%></td>
		        <td><%#Eval("ContactPhone")%></td>
		        <td><div class="date"><%#Eval("LastLoginTime")%></div></td>
	            <td><div class="date"><%#Eval("RegisterTime")%></div></td>
	            <td><div class="date"><%#Eval("AuditTime")%></div></td>
                    <td><%#Eval("PeriodStartOfUse")%><%#Eval("Spliter") %><br /><%#Eval("PeriodEndOfUse")%></td>--%>
		        <td><%#Eval("AuditedState")%></td>
                <td><%#(bool)Eval("Enabled")?"正常":"禁用"%></td>
                <td><%#Eval("RegisterTime")%><br/><%#Eval("LastLoginTime")%></td>
	            <td class="controls">
	                <span style='display:<%#(CompanyType)Eval("CompanyType") != CompanyType.Platform ? "" : "none;" %>'>
	                <div style=' display:<%#(bool)Eval("IsOem")?"none;":""%>'>
                           <%--<asp:LinkButton Text='<%#(bool)Eval("Enabled")?"禁用帐号":"启用帐号" %>' 
                             CommandName='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' 
                             CommandArgument='<%#Eval("CompanyId") %>' runat="server"/>--%>
                             <a href='javascript:void(0);' class='<%#(bool)Eval("Enabled")?"Disable":"Enable" %>' companyAccount='<%#Eval("UserNo")%>' companyId='<%#Eval("CompanyId") %>' ><%#(bool)Eval("Enabled")?"禁用帐号":"启用帐号" %></a>
                   </div>

			       <div style="display:<%#(CompanyType)Eval("CompanyType") != CompanyType.Purchaser ? "" : "none;" %>"> <a href="PolicySet.aspx?CompanyId=<%#Eval("CompanyId") %>" class="el-text">政策设置</a></div>

			       <%--<div style='display:<%#(CompanyType)Eval("CompanyType") != CompanyType.Purchaser&&(bool)Eval("AuditEnable") ? "" : "none;" %>'> <a  href="CompanyAudit.aspx?CompanyId=<%#Eval("CompanyId") %>" class="el-text">审核</a></div>--%>
                   
                   <div><a href='./CompanyInfoMaintain.aspx?CompanyId=<%#Eval("CompanyId") %>&AccountType=<%#Eval("AccountTypeValue") %>' class="el-text">基础信息</a></div>
                   <div><a href="<%#Eval("CompanyType").ToString()==CompanyType.Supplier.ToString()?"UpdateProductWorkInfo.aspx":Eval("CompanyType").ToString()==CompanyType.Purchaser.ToString()?"UpdateBuyWorkInfo.aspx":"UpdateOutWorkInfo.aspx"%>?CompanyId=<%#Eval("CompanyId") %>&CompanyType=<%#Eval("CompanyTypeValue") %>" class="el-text">工作信息</a></div>

			        <div><a href="../../RoleInfoAddModify/System_RoleCompany.htm?CompanyId=<%#Eval("CompanyId") %>&CompanyType=<%#((int)(CompanyType)Eval("CompanyType")).ToString() %>" class="el-text">权限设置</a></div>

			        <div><a href="javascript:ResetCompanyPWd('<%#Eval("UserNo") %>')" class="el-text">重置密码</a></div>

			        <div><a href="LookUpComapanyRelation.aspx?CompanyId=<%#Eval("CompanyId") %>" class="el-text">关联关系</a></div>
                    <div><a href="#" class="merchant" tip='<%#Eval("CompanyId") %>' accountType='<%#Eval("AccountTypeValue") %>'>设为经纪人</a></div>
                  </span>
		        </td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="layers" id="test_layer" style="display: none;">
        <div>
            <h2>
                重置密码原因：</h2>
            <asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px"
                Height="60px" MaxLength="500" />
        </div>
        <div class="btns">
            <asp:HiddenField runat="server" ID="hdCompanyAccount" />
            <asp:Button ID="btnReset" runat="server" CssClass="class1 btn" Text="重置" onclick="btnReset_Click"
                OnClientClick="return CheckReason()" />
            <input type="button" value="返回" class="btn class2" id="btnGoBack" onclick="CancleReset()" />
        </div>
    </div>
    <div class="fixed" style="z-index:60">
    </div>
      <a id="divOpcial" style="display: none;" data="{type:'pop',id:'div_Merchant'}"></a>
        <div id="div_Merchant" class="form layer2" style="display: none;">
           <h4>请填写经纪人基本信息：</h4>
           <table>
             <tr>
               <td class="title">Pos费率：</td>
               <td><asp:TextBox ID="txtPosRate" runat="server" CssClass="text"></asp:TextBox>%</td>
               <td><asp:Label ID="lblWarnRateInfo" runat="server" CssClass="warn"></asp:Label></td>
             </tr>
             <tr class="certNo">
               <td class="title certNo">联系人身份证号：</td>
               <td><asp:TextBox ID="txtContactCertNo" runat="server" CssClass="certNo text"></asp:TextBox></td>
               <td><asp:Label ID="lblWarnCertNoInfo" runat="server" CssClass="warn"></asp:Label></td>
             </tr>
           </table>
           <div class="btns">
              <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" OnClick="btnSave_Click" />
              <input type="button" class="btn class2 close"  value="取消"/>
           </div>
        </div>
      <a id="divMain" style="display: none;" data="{type:'pop',id:'div_Refuse'}"></a>
        <div class="layer3 hidden" id="div_Refuse">
        <h4>禁用账号操作<a href="javascript:;" class="close">关闭</a></h4>
        <div class="con">
            <p class="tips mar">请在下方输入您的禁用账号理由或备注。</p>
            <textarea class="text" cols="105" rows="4" id="txtReason"></textarea>
        </div>
           <div class="btns">
              <input type="button" class="btn class1" id="btnConfirm" value="确定" />
              <input type="button" class="btn class2 close"  value="取消"/>
           </div>
        </div>
        <asp:HiddenField  ID="hfdCompanyId" runat="server"/>
        <asp:HiddenField ID="hfdSearchCondition" runat="server" />
        <asp:HiddenField ID="hfdDisableCompanyId" runat="server" />
        <asp:HiddenField ID="hfdCompanyAccount" runat="server" />
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/FixTable.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/TerraceModule/CompanyList.aspx.min.js?2012122202" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".Disable").click(function () {
            $("#divMain").click();
            $("#hfdDisableCompanyId").val($(this).attr("companyId"));
            $("#hfdCompanyAccount").val($(this).attr("companyAccount"));
        });
        $("#btnConfirm").click(function () {
            if (valiateReason()) {
                var companyId = $("#hfdDisableCompanyId").val();
                var companyAccount = $("#hfdCompanyAccount").val();
                var reason = $("#txtReason").val();
                sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/Disable", JSON.stringify({ "companyId": companyId, "companyAccount": companyAccount, "reason": reason }),
            function () {
                alert("禁用账号成功");
                $("#btnQuery").click();
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
            }
        });
        $(".Enable").click(function () {
            var companyId = $(this).attr("companyId");
            sendPostRequest("/OrganizationHandlers/CompanyInfoMaintain.ashx/Enable", JSON.stringify({ "companyId": companyId }), function () {
                alert("启用账号成功");
                $("#btnQuery").click();
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
        });
    })
    function valiateReason() {
        if ($.trim($("#txtReason").val()).length == 0) {
            alert("请输入禁用账号理由或备注");
            return false;
        }
        if ($.trim($("#txtReason").val()).length > 200) {
            alert("禁用账号理由或备注字数不能超过200");
            return false;
        }
        return true;
    }
</script>