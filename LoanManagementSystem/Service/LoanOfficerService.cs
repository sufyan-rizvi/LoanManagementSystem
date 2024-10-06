using System;
using System.Collections.Generic;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Models;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace LoanManagementSystem.Service
{
    public class LoanOfficerService : ILoanOfficerService
    {
        private static ILoanOfficerRepository _loanOfficerRepository;
        private static ICustomerRepository _customerRepository;
        public LoanOfficerService(ILoanOfficerRepository loanOfficerRepository, ICustomerRepository customerRepository)
        {
            _loanOfficerRepository = loanOfficerRepository;
            _customerRepository = customerRepository;
        }

        public List<LoanApplication> GetDocuments()
        {
            return _loanOfficerRepository.GetAllDocuments();
        }
        public List<RegistrationDocuments> GetAppDocuments(Guid id)
        {
            return _loanOfficerRepository.GetAppDocuments(id);
        }
        public void RegApproveLoan(Guid id)
        {
            _loanOfficerRepository.RegApproveLoan(id);
            var application = _customerRepository.GetApplicationById(id);
            SendRegistrationApprovalEmail(application);
        }
        public void RejectApproveLoan(Guid id, string comments)
        {
            _loanOfficerRepository.RejectApproveLoan(id, comments);
            var application = _customerRepository.GetApplicationById(id);
            SendLoanRejectionEmail(application);
        }
        public List<LoanApplication> GetCollateralDocuments()
        {
            return _loanOfficerRepository.GetCollateralDocuments();
        }
        public List<CollateralDocuments> GetToShowCollateralDocuments(Guid id)
        {
            return _loanOfficerRepository.GetToShowCollateralDocuments(id);
        }
        public void ApproveCollateralDocuments(Guid id, string comments)
        {
            _loanOfficerRepository.ApproveCollateralDocuments(id, comments);
            var application = _customerRepository.GetApplicationById(id);
            SendRegistrationApprovalEmail(application);
        }
        public void RejectCollateralDocuments(Guid id, string comments)
        {
            _loanOfficerRepository.RejectCollateralDocuments(id, comments);
            var application = _customerRepository.GetApplicationById(id);
            SendLoanRejectionEmail(application);
        }

        private static void SendRegistrationApprovalEmail(LoanApplication loanApplication)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
                client.Timeout = 10000;
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                    Subject = "Loan Approved !!",
                    Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            background-color: #ffffff;
            margin: 0 auto;
            padding: 20px;
            max-width: 600px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        .header {
            background-color: #4CAF50;
            color: white;
            padding: 10px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }
        .content {
            padding: 20px;
        }
        .content h1 {
            font-size: 24px;
            color: #333;
        }
        .content p {
            font-size: 16px;
            line-height: 1.6;
            color: #555;
        }
        .content a {
            color: #4CAF50;
            text-decoration: none;
        }
        .footer {
            text-align: center;
            padding: 10px;
            font-size: 12px;
            color: #888;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Loan Approved!</h1>
        </div>
        <div class='content'>
            <h1>Dear " + loanApplication.Applicant.User.FirstName + @",</h1>
            

            
            <p>We are excited to inform you that your loan application has been approved. Congratulations on this achievement!</p>
            <p><strong>Loan Details:</strong></p>
            <ul>
                <li><strong>Loan Amount:</strong> $" + loanApplication.LoanAmount.ToString("C", new CultureInfo("en-IN")) + @"</li>
                <li><strong>Interest Rate:</strong> " + loanApplication.Scheme.InterestRate + @"%</li>
                <li><strong>Loan Term:</strong> " + loanApplication.Tenure + @" months</li>
                <li><strong>Approval Date:</strong> " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
            </ul>

            <p><strong>Payment Details:</strong></p>
            <ul>
                <li><strong>Repayment Amount:</strong> $" + loanApplication.EMIAmount.ToString("C", new CultureInfo("en-IN")) + @"</li>
                <li><strong>Payment Date:</strong> " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
            </ul>

            <h3>What's Next?</h3>
            <p>Your next payment is scheduled for <strong>" + loanApplication.NextPaymentDate.ToString("dd/MM/yyyy") + @"</strong>. You can always log in to your <a href='#'>Aksys Loans</a> account to view your repayment schedule, track your loan status, and manage your account.</p>
            <p>Thank you for staying on top of your repayments and for choosing <strong>Aksys Loans</strong> to manage your financial needs. We appreciate your trust in us.</p>
            <h3>Need Help?</h3>
            <p>If you have any questions, feel free to contact our support team at <a href='mailto:support@aksysloans.com'>support@aksysloans.com</a> or call us at <strong>+123 456 7890</strong>.</p>
            <p>We look forward to continuing to assist you with your financial needs.</p>
            <p>Thank you,</p>
            <p><strong>The Aksys Loans Team</strong></p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Aksys Loans. All rights reserved.</p>
        </div>
    </div>
</body>
</html>",
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };
                mailMessage.To.Add(loanApplication.Applicant.User.Email);

                client.Send(mailMessage);
            }

        }


        private static void SendLoanRejectionEmail(LoanApplication loanApplication)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("dfgutdxvhuyf@gmail.com", "gpjb izjt kmfs tnzs");
                client.Timeout = 10000;
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("dfgutdxvhuyf@gmail.com"),
                    Subject = "Loan Application Rejected",
                    Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            background-color: #ffffff;
            margin: 0 auto;
            padding: 20px;
            max-width: 600px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
        .header {
            background-color: #f44336;
            color: white;
            padding: 10px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }
        .content {
            padding: 20px;
        }
        .content h1 {
            font-size: 24px;
            color: #333;
        }
        .content p {
            font-size: 16px;
            line-height: 1.6;
            color: #555;
        }
        .content a {
            color: #f44336;
            text-decoration: none;
        }
        .footer {
            text-align: center;
            padding: 10px;
            font-size: 12px;
            color: #888;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Loan Application Rejected</h1>
        </div>
        <div class='content'>
            <h1>Dear " + loanApplication.Applicant.User.FirstName + @",</h1>
            <p>We regret to inform you that your loan application has been rejected. We understand that this may be disappointing news, and we want to assure you that this decision was made after careful consideration.</p>
            
            <h2>Reason for Rejection</h2>
            <p>Your application was reviewed based on several criteria, and unfortunately, it did not meet our lending requirements. Some common reasons for rejection include:</p>
            <ul>
                <li>Insufficient income</li>
                <li>High debt-to-income ratio</li>
                <li>Credit history concerns</li>
            </ul>

            <h3>What to Do Next?</h3>
            <p>You are welcome to review your application and address any issues before applying again. We recommend improving your credit score and ensuring your financial situation aligns better with our requirements.</p>
            <p>If you have any questions or need further assistance, please do not hesitate to contact our support team at <a href='mailto:support@aksysloans.com'>support@aksysloans.com</a> or call us at <strong>+123 456 7890</strong>.</p>
            <p>We appreciate your interest in <strong>Aksys Loans</strong> and hope to serve you in the future.</p>
            <p>Thank you,</p>
            <p><strong>The Aksys Loans Team</strong></p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Aksys Loans. All rights reserved.</p>
        </div>
    </div>
</body>
</html>",
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };

                mailMessage.To.Add(loanApplication.Applicant.User.Email);

                client.Send(mailMessage);
            }
        }

    }
}