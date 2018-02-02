<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" EnableViewState="false" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage.AddEmployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加员工</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <h3 class="titleBg">添加员工</h3>
        <form id="form" runat="server">
        <table>
            <colgroup>
                <col class="w20" />
                <col  />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">
                        姓名：
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="text"></asp:TextBox>
                        <span id="lblName"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        性别
                        ：</td>
                    <td>
                        <div class="check">
                            <asp:RadioButton ID="rdoMan" runat="server" Text="男" GroupName="sex" Checked="true"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoWoMan" runat="server" Text="女" GroupName="sex" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        用户名：
                    </td>
                    <td>
                       <asp:TextBox ID="txtAccountNo" runat="server" CssClass="text"></asp:TextBox>
                       <input type="button" class="class3 btn" value="验证用户名" id="btnCheCkAccounNo"/>
                       <span id="lblAccountNo"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        密码：
                    </td>
                    <td>
                       <asp:TextBox ID="txtPassword" runat="server" CssClass="text" TextMode="Password" onpaste="return false"></asp:TextBox>
                       <span id="lblPassword"></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                       确认密码：
                    </td>
                    <td>
                       <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text" TextMode="Password" onpaste="return false"></asp:TextBox>
                       <span id="lblConfirmPassword"></span>
                    </td>
                </tr>     
                <tr>
                    <td class="title">
                        手机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCellPhone" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips"><i class="blue icon icon-info-circle"></i></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        座机：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips"><i class="blue icon icon-info-circle"></i></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        E-mail：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                        <span class="tips"><i class="blue icon icon-info-circle"></i></span>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        状态：
                    </td>
                    <td>
                        <div class="check">
                            <asp:RadioButton ID="rdoEnabled" runat="server" Text="启用"  GroupName="state" Checked="true"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoOnEnabled" runat="server" Text="禁用" GroupName="state" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        备注：
                    </td>
                    <td>
                        <textarea class="text" cols="80" runat="server" id="remark" rows="4"></textarea>
                        <span></span>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <asp:Button ID="btnSave" runat="server" CssClass="class1 btn" Text="保存" onclick="btnSave_Click" />
            <asp:Button ID="btnSaveAndContinue" runat="server" CssClass="class1 btn" Text="保存并且继续" onclick="btnSaveAndContinue_Click"/>
            <input type="button" onclick="return window.location.href='./StaffInfoMgr.aspx'" class="btn class2" value="返回" />
        </div>
        </form>
    </div>
    <script src="../../../Scripts/json2.js" type="text/javascript"></script>
    <script src="../../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/Employee/Employee.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/Checking.js" type="text/javascript"></script>
</body>
</html>
