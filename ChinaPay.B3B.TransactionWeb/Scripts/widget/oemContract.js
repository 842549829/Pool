/*
绑定Contract信息
*/
function bindContract(servicePhone, enterpriseQQ, fax, refundPhone, scrapPhone, payServicePhone, emergencyPhone, complainPhone, printTicketPhone) {
    var assignValue = function (controlId, value) {
        $("#" + controlId).text(value);
    };
    sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/CurrentContract", null,
    function (result) {
        if (servicePhone) {
            assignValue(servicePhone, result.ServicePhone);
        }
        if (enterpriseQQ) {
            assignValue(enterpriseQQ, result.EnterpriseQQ);
        }
        if (fax) {
            assignValue(fax, result.Fax);
        }
        if (refundPhone) {
            assignValue(refundPhone, result.RefundPhone);
        }
        if (scrapPhone) {
            assignValue(scrapPhone, result.ScrapPhone);
        }
        if (payServicePhone) {
            assignValue(payServicePhone, result.PayServicePhone);
        }
        if (emergencyPhone) {
            assignValue(emergencyPhone, result.EmergencyPhone);
        }
        if (complainPhone) {
            assignValue(complainPhone, result.ComplainPhone);
        }
        if (printTicketPhone) {
            assignValue(printTicketPhone, result.PrintTicketPhone);
        }
    },
    function () {
        var defaultPhone = "0871-68883388";
        if (servicePhone) {
            assignValue(servicePhone, defaultPhone);
        }
        if (enterpriseQQ) {
            assignValue(enterpriseQQ, defaultPhone);
        }
        if (fax) {
            assignValue(fax, defaultPhone);
        }
        if (refundPhone) {
            assignValue(refundPhone, defaultPhone);
        }
        if (scrapPhone) {
            assignValue(scrapPhone, defaultPhone);
        }
        if (payServicePhone) {
            assignValue(payServicePhone, defaultPhone);
        }
        if (emergencyPhone) {
            assignValue(emergencyPhone, defaultPhone);
        }
        if (complainPhone) {
            assignValue(complainPhone, defaultPhone);
        }
        if (printTicketPhone) {
            assignValue(printTicketPhone, defaultPhone);
        }
    });
}
