<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateEmployee.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage.UpdateEmployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改员工</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
  <div class="form">
        <h3 class="titleBg">修改员工</h3>
        <form id="form1" runat="server">
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
                         <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        性别
                        ：</td>
                    <td>
                        <div class="check">
                            <asp:RadioButton ID="rdoMan" runat="server" Text="男" GroupName="sex"/>
                            <asp:RadioButton ID="rdoWoan" runat="server" Text="女" GroupName="sex" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        用户名：
                    </td>
                    <td>
                       <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
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
                            <asp:RadioButton ID="rdoEnabled" runat="server" Text="启用"  GroupName="state"/>
                            <asp:RadioButton ID="rdoOnEnabled" runat="server" Text="禁用" GroupName="state" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        备注：
                    </td>
                    <td>
                        <textarea class="text" runat="server" id="remark" cols="80" rows="6"></textarea>
                        <span></span>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btns">
            <asp:Button ID="btnSave" runat="server" CssClass="class1 btn" Text="保存" onclick="btnSave_Click" />
            <input type="button" onclick="return window.location.href='./StaffInfoMgr.aspx'" class="btn class2" value="返回" />
        </div>
        </form>
    </div>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/Employee/Employee.js" type="text/javascript"></script>
</body>
</html>
