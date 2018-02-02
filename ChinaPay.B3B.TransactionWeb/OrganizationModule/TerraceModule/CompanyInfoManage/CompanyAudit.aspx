<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyAudit.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.CompanyAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="form">
			<h2>公司基础信息</h2>
			<form id="form" runat="server">
				<table>
					<colgroup>
						<col class="w15" />
						<col class="w35" />
                        <col class="w15" />
                        <col class="35" />
					</colgroup>
					<tr>
						<td class="title">用户名:</td>
						<td>
						    <asp:Label ID="lblAccuountNo" runat="server"></asp:Label>
						</td>
                        <td class="title">公司类型</td>
                        <td>
                            <asp:Label ID="lblCompanyType" runat="server"></asp:Label>
                        </td>
					</tr>
                    <tr>
                    	<td class="title">公司名称</td>
                        <td>
                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                        </td>
                        <td class="title">公司简称</td>
                        <td>
                            <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                        </td>
                    </tr>					
                    <tr>
                    	<td class="title">公司地址</td>
                        <td>
                            <asp:Label ID="lblCompanyAddress" runat="server"></asp:Label>
                        </td>
                        <td class="title">使用权限</td>
                        <td>
                            <asp:Label ID="lblBeginDeadline" runat="server" Text="2012-12-12"></asp:Label>至
                            <asp:Label ID="lblEndDeadline" runat="server" Text="2012-12-12"></asp:Label>
                        </td>
                    </tr>	                    
                    
                     <tr>
                    	<td class="title">所在地</td>
                        <td>
                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                        </td>
                        <td class="title">邮政编码</td>
                        <td>
                            <asp:Label ID="lblPostCode" runat="server"></asp:Label>
                        </td>
                    </tr>	
                    <tr>
                    	<td class="title">公司电话</td>
                        <td>
                            <asp:Label ID="lblCompanyPhone" runat="server"></asp:Label>
                        </td>
                        <td class="title">传真</td>
                        <td>
                            <asp:Label ID="lblFaxes" runat="server"></asp:Label>
                        </td>
                    </tr>	
                    <tr>
                        <td class="title">
                            业务联人
                        </td>
                        <td>
                            <asp:Label ID="lblLinkMan" runat="server"></asp:Label>
                        </td>
                        <td class="title">
                            业务联系人电话
                        </td>
                        <td>
                            <asp:Label ID="lblLinkManPhone" runat="server"></asp:Label>
                        </td>
                    </tr>                                 
                    <tbody id="tbAgen" runat="server">
                        <tr>
                    	    <td class="title">负责人</td>
                            <td>
                                <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                            </td>
                            <td class="title">负责人电话</td>
                            <td>
                                <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                紧急联系人
                            </td>
                            <td>
                                <asp:Label ID="lblUrgencyLinkman" runat="server"></asp:Label>
                            </td>
                            <td class="title">
                                紧急联系人电话
                            </td>
                            <td>
                                <asp:Label ID="lblUrgencyLinkmanPhone" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
					<tr>
						<td class="title">Email</td>
						<td>
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
						</td>
                        <td class="title">QQ</td>
                        <td>
                           <asp:Label ID="lblQQ" runat="server"></asp:Label>

                        </td>
					</tr>
					<tr>
						<td class="title">MSN</td>
						<td>
                            <asp:Label ID="lblMSN" runat="server"></asp:Label>
						</td>
                        <td class="title"></td>
                        <td>
                        </td>
					</tr>  
                    <tbody id="tbAgens" runat="server">
                        <tr>
                            <td colspan="4" style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                航协经营批准号
                            </td>
                            <td>
                                <asp:Label ID="lblIATABusinessApprovalNumber" runat="server"></asp:Label>
                            </td>
                            <td class="title">
                                IATA号
                            </td>
                            <td>
                                <asp:Label ID="lblTATANumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="title">
                                中航协担保金
                            </td>
                            <td>
                                <asp:Label ID="lblCaticAssociationSuch" runat="server"></asp:Label>
                            </td>
                            <%--   <td class="title">业务类型</td>
                        <td>
                             <asp:CheckBoxList ID="chklBusinessType" runat="server" RepeatColumns="8" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false"></asp:CheckBoxList>
                        </td>--%>
                            <td class="title">
                                航空代理资质
                            </td>
                            <td>
                                <asp:RadioButton ID="rdoFirst" Text="一类" runat="server" GroupName="Airline" />&nbsp;
                                <asp:RadioButton ID="rdoTwo" Text="二类" runat="server" GroupName="Airline" />&nbsp;
                                <asp:RadioButton ID="rdoLast" Text="三类" runat="server" GroupName="Airline" />
                            </td>
                        </tr>
                    </tbody>
					<tr class="btns">
						<td colspan="4">
							<asp:Button ID="btnAccept" runat="server" CssClass="btn class1" Text="审核通过" onclick="btnAccept_Click" />
                            <asp:Button ID="btnReject" runat="server" CssClass="btn class1" Text="审核拒绝" onclick="btnReject_Click" />
							<input class="btn class2" type="button" value="返回" onclick="return window.location.href='./CompanyList.aspx?Search=Back';" />
						</td>
					</tr>
				</table>
			</form>
		</div>		
</body>
</html>
