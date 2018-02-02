<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountInformation.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.Account.AccountInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>账户信息</title>
    
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head><link href="/Styles/register.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
    <!--账户信息-->
    <div id="smallbd1">
        <h3 class="titleBg">账户信息</h3>
        <div class="account_box">
           <p>国付通可以帮助您实现安全、便捷的网络支付，购票时可以使用国付通支付</p>
            <div class="account_list_box" id="account_list_box">
				<ul class="account_list clearfix">
					<li class="curr" id="liShowPay">
						<a href="javascript:void(0);" class="fl text-c account_name jiebang_ico">
							<i class="account_ico pay_ico jiebang_ico"></i>
                            <span id="pay" runat="server"></span>
						</a>
					</li>
					<li runat="server" id="liShowCollection">
						<a href="javascript:void(0);" class="fl text-c account_name jiebang_ico">
							<i class="account_ico collection_ico jiebang_ico"></i>
                            <span id="collection" runat="server"></span>
						</a>
					</li>
					<li runat="server" id="liHideCollection">
						<a href="javascript:void(0)" class="jiebang_ico account_add_btn" id="add" runat="server">添加国付通收款账号</a>
					</li>
				</ul>
			</div>
        </div>
        <div class="account_table clearfix" id="divPay">
			<div class="fl account_info_box">
                <h3 class="titleBg">付款账户</h3>
				<table class="accountInfo">
					<tr>
						<td class="title">绑定的付款账户：</td>
						<td>
                            <asp:Label runat="server" ID="lblPayAccountNo"></asp:Label>
                            <a href="javascript:payReturnUrl(0)" class="gotoPoolpay">进入国付通</a>
                        </td>
					</tr>
					<tr>
						<td class="title">账户名：</td>
						<td><asp:Label runat="server" ID="lblPayAccountName"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">地址：</td>
						<td>
                            <asp:Label runat="server" ID="lblPayAddress"></asp:Label>
                            <a href="javascript:payReturnUrl(1)" class="correct">修改</a>
                        </td>
					</tr>
					<tr>
						<td class="title">邮编：</td>
						<td><asp:Label runat="server" ID="lblPayPostCode"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">身份证号：</td>
						<td><a href="javascript:payReturnUrl(2)">查看</a></td>
					</tr>
					<tr>
						<td class="title">管理员姓名：</td>
						<td><asp:Label runat="server" ID="lblPayAdminName"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">管理员手机：</td>
						<td><a href="javascript:payReturnUrl(3)">查看</a></td>
					</tr>
					<tr>
						<td class="title">管理员邮箱：</td>
						<td><asp:Label runat="server" ID="lblPayAdminEmail"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">注册时间：</td>
						<td><asp:Label runat="server" ID="lblPayRegisterTime"></asp:Label></td>
					</tr>
				</table>
			</div>
			<div class="fr account_exc_box">
                <h3 class="titleBg">余额信息</h3>
				<table class="balanceInfo">
					<tr>
						<td class="title">账户余额:</td>
						<td><a href="javascript:payReturnUrl(4)">查看</a>|<a href="javascript:payReturnUrl(5)">充值</a></td>
					</tr>
					<tr>
						<td class="title">状态：</td>
						<td><asp:Label runat="server" ID="lblPayStatus"></asp:Label></td>
					</tr>
				</table>
			</div>
		</div>
        <div class="account_table clearfix" id="divCollection" runat="server" style="display:none;">
			<div class="fl account_info_box">
                <h3 class="titleBg">收款账户</h3>
				<table class="accountInfo">
					<tr>
						<td class="title">绑定的收款账户：</td>
						<td>
                            <asp:Label runat="server" ID="lblCollectionAccountNo"></asp:Label>
                            <a href="javascript:collectionReturnUrl(0)" class="gotoPoolpay">进入国付通</a>
                            <a href="javascript:void(0);"  class="correct" id="replacementOpen" runat="server">更换收款账号</a>
                        </td>
					</tr>
					<tr>
						<td class="title">账户名：</td>
						<td><asp:Label runat="server" ID="lblCollectionAccountName"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">地址：</td>
						<td><asp:Label runat="server" ID="lblCollectionAddress"></asp:Label>
                        <a href="javascript:collectionReturnUrl(1)" class="correct">修改</a></td>
					</tr>
					<tr>
						<td class="title">邮编：</td>
						<td><asp:Label runat="server" ID="lblCollectionPostCode"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">身份证号：</td>
						<td><a href="javascript:collectionReturnUrl(2)" >查看</a></td>
					</tr>
					<tr>
						<td class="title">管理员姓名：</td>
						<td><asp:Label runat="server" ID="lblCollectionAdminName"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">管理员手机：</td>
						<td><a href="javascript:collectionReturnUrl(3)">查看</a></td>
					</tr>
					<tr>
						<td class="title">管理员邮箱：</td>
						<td><asp:Label runat="server" ID="lblCollectionAdminEmail"></asp:Label></td>
					</tr>
					<tr>
						<td class="title">注册时间：</td>
						<td><asp:Label runat="server" ID="lblCollectionRegisterTime"></asp:Label></td>
					</tr>
				</table>
			</div>
			<div class="fr account_exc_box">
                <h3 class="titleBg">余额信息</h3>
				<table class="balanceInfo">
					<tr>
						<td class="title">账户余额:</td>
						<td><a href="javascript:collectionReturnUrl(4)">查看</a>|<a href="javascript:collectionReturnUrl(5)">充值</a></td>
					</tr>
					<tr>
						<td class="title">状态：</td>
						<td><asp:Label runat="server" ID="lblCollectionStatus"></asp:Label></td>
					</tr>
				</table>
			</div>
		</div>
    </div>
    <!--更换账号-->
    <a id="replacement" style="display: none;" data="{type:'pop',id:'replacement_account'}"></a>
    <div id="replacement_account" runat="server" class="layer_box account_layer_box" style="display: none;">
		<div class="layer_hd">
			<h4>更换国付通收款账号</h4>
			<a href="javascript:void(0)" title="关闭" class="close confirmClose">关闭</a>
		</div>
		<div class="layer_tips text-c obvious">
			<i class="ico_important"></i>
			建议在申请更换账号前将您的机票订单处理完毕以避免因退改签等发送的机票分账纠纷。
		</div>
		<div class="layer_table form">
			<table>
                <colgroup>
                    <col class="w30" />
                    <col />
                </colgroup>
				<tr>
                    <td class="title">原收款账号：</td>
					<td><span id="lblOriginal"></span></td>
				</tr>
				<tr>
					<td class="title">新收款账号：</td>
					<td>
						<input type="text" class="text" id="txtNewPayAccount" />
						<span>请认真核对后输入</span>&nbsp;&nbsp;
                        <a id="registerAccountNoOpen" href="javascript:void(0)">没有账号？点此注册</a>
					</td>
				</tr>
				<tr>
					<td class="title">新账号支付密码：</td>
					<td>
						<input type="password" class="text" id="txtNewPayPassword" />
						<span>请输入新收款账号的支付密码</span>
					</td>
				</tr>
			</table>
		</div>
		<div class="btns">
			<input type="button" value="确认更换" id="btnReplacement" class="btn class1" />
			<input type="button" value="取&nbsp;&nbsp;&nbsp;消" class="btn class2 close"/>
		</div>
	</div>
    <!--注册账号--->
    <a id="registerAccountNo" runat="server" style="display: none;" data="{type:'pop',id:'divRegisterAccountNo'}"></a>
    <div class="layer_box account_layer_box" id="divRegisterAccountNo" style="display: none;">
		<div class="layer_hd">
			<h4>注册新的收款账号</h4>
			<a href="javascript:void(0)" title="关闭" class="close confirmClose1">关闭</a>
		</div>
		<div class="layer_table form">
			<table>
                <colgroup><col class="w25" /><col /></colgroup>
                <tbody>
				    <tr>
					    <td class="title">请选择账户类型</td>
					    <td>
						    <input type="radio" id="rdoIndividual" checked="checked" name="rdo"/>
						    <label for="rdoIndividual">个人账户</label>
						    <input type="radio" id="rdoEnterprise" name="rdo" />
						    <label for="rdoEnterprise">企业账户</label> 
					    </td>
				    </tr>
				    <tr>
					    <td class="title">国付通账号：</td>
					    <td>
						    <input type="text" class="text" id="txtRegisterAccountNo" />
						    <span></span>
					    </td>
				    </tr>
				    <tr>
					    <td class="title">登录密码：</td>
					    <td>
						    <input type="password" class="text" id="txtLoginPassword" />
						    <span></span>
					    </td>
				    </tr>
				    <tr>
					    <td class="title">确认登录密码：</td>
					    <td>
						    <input type="password" class="text" id="txtConfirmLoginPassword" />
						    <span></span>
					    </td>
				    </tr>
				    <tr>
					    <td class="title">支付密码：</td>
					    <td>
						    <input type="password" class="text" id="txtRegisterPayPassword" />
						    <span></span>
					    </td>
				    </tr>
				    <tr>
					    <td class="title">确认支付密码：</td>
					    <td>
						    <input type="password" class="text" id="txtConfirmRegisterPayPassword" />
						    <span></span>
					    </td>
				    </tr>
                </tbody>
                <tbody id="preson">
                    <tr>
					<td class="title">真实姓名：</td>
					<td>
						<input type="text" class="text" id="txtName" />
						<span></span>
					</td>
				    </tr>
				    <tr>
					    <td class="title">身份证号：</td>
					    <td>
						    <input type="text" class="text" id="txtIDCard" />
						    <span></span>
					    </td>
				    </tr>
				    <tr>
					    <td class="title">联系人手机号码：</td>
					    <td>
						    <input type="text" class="text" id="txtPresonCellPhone" />
						    <span></span>
					    </td>
				    </tr>
                </tbody>
                <tbody id="company" style="display:none;">
                    <tr>
                        <td class="title">企业名称：</td>
                        <td>
                            <input type="text" class="text" id="txtCompanyName"/>
                             <span></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">机构代码：</td>
                        <td>
                            <input type="text" class="text" id="txtOrganizationCode"/>
                             <span></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">法人手机号：</td>
                        <td>
                            <input type="text" class="text" id="txtCompanyPhone"/>
                             <span></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">法人姓名：</td>
                        <td>
                            <input type="text" class="text" id="txtLegalPersonName"/>
                            <span></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">联系人手机号：</td>
                        <td>
                            <input type="text" class="text" id="txtCompanyCellPhone"/>
                                <span></span>
                        </td>
                    </tr>
                </tbody>
			</table>
		</div>
		<div class="btns">
			<input type="button" id="btnConfirmPayAccountNo" value="确认并以此作为收款账号" class="btn class1" />
			<input type="button" value="取&nbsp;&nbsp;&nbsp;消" class="btn class2 close" />
		</div>
	</div>
    <!--更换账号成功页面-->
    <a id="replacementSuccess" style="display:none;" data="{type:'pop',id:'confirmSuccess'}"></a>
    <div class="layer_box account_layer_box" id="confirmSuccess" style="display:none;">
		<div class="layer_hd">
			<h4>更换国付通收款账号</h4>
			<a href="javascript:void(0)" title="关闭" class="close"  id="exit">关闭</a>
		</div>
		<div class="layer_tips text-c obvious">
			<i class="ico_success"></i>
			<span>收款账号更换成功</span>
			<br />
			<span class="b">新收款账号：</span><span id="lblNewAccountNo"></span>
		</div>
		<div class="btns">
			<input type="button" value="确定" id="btnExit"  class="btn class1" />
		</div>
	</div>
    <!--注册账号-->
    <a id="registerSuccess" style="display:none;" data="{type:'pop',id:'confirmRegisterSuccess'}"></a>
    <div class="layer_box account_layer_box" id="confirmRegisterSuccess" style="display:none">
		<div class="layer_hd">
			<h4>注册国付通收款账号</h4>
			<a href="javascript:void(0)" title="关闭" class="close"  id="exit1">关闭</a>
		</div>
		<div class="layer_tips text-c obvious">
			<i class="ico_success"></i>
			<span>收款账号注册成功</span>
			<br />
			<span class="b">新收款账号：</span><span id="lblRegisterNewAccount"></span>
		</div>
		<div class="btns">
			<input type="button" value="确定" id="btnExit1"  class="btn class1" />
		</div>
	</div>
    </form>
</body>
</html>
<script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
<script src="/Scripts/OrganizationModule/Account/MyPoolPay.js?20130522" type="text/javascript"></script>