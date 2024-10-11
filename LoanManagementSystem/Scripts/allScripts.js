//document.getElementById("calculateBtn").addEventListener('click', function () {
//    // Get values from the input fields
//    const loanAmount = parseFloat(document.getElementById('loanAmount').value);
//    console.log(loanAmount)
//    const annualInterestRate = parseFloat(document.getElementById('interestRate').value);
//    const loanTenureYears = parseInt(document.getElementById('loanTenure').value);

//    // Get error message elements
//    const loanAmountError = document.getElementById("loanAmountError");
//    const interestRateError = document.getElementById("interestRateError");
//    const loanTenureError = document.getElementById("loanTenureError");

//    // Initialize valid flag
//    let isValid = true;

//    // Loan Amount Validation: between ₹1,000 and ₹10,000,000
//    if (loanAmount < 1000 || loanAmount > 10000000 || isNaN(loanAmount)) {
//        loanAmountError.style.display = "inline";
//        isValid = false;
//    } else {
//        loanAmountError.style.display = "none";
//    }

//    // Interest Rate Validation: between 0% and 100%
//    if (annualInterestRate < 0 || annualInterestRate > 100 || isNaN(annualInterestRate)) {
//        interestRateError.style.display = "inline";
//        isValid = false;
//    } else {
//        interestRateError.style.display = "none";
//    }

//    // Loan Tenure Validation: between 1 and 30 years
//    if (loanTenureYears < 1 || loanTenureYears > 30 || isNaN(loanTenureYears))
