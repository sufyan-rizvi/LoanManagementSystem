document.getElementById('calculateBtn').addEventListener('click', function () {
    // Get values from the input fields
    const loanAmount = parseFloat(document.getElementById('loanAmount').value);
    const annualInterestRate = parseFloat(document.getElementById('interestRate').value);
    const loanTenureYears = parseInt(document.getElementById('loanTenure').value);

    // Validation for empty inputs
    if (isNaN(loanAmount) || isNaN(annualInterestRate) || isNaN(loanTenureYears)) {
        alert("Please fill in all fields with valid numbers");
        return;
    }

    // Convert annual interest rate to monthly interest rate
    const monthlyInterestRate = (annualInterestRate / 100) / 12;

    // Convert loan tenure to months
    const loanTenureMonths = loanTenureYears * 12;

    // EMI Formula: [P * r * (1 + r)^n] / [(1 + r)^n - 1]
    const emi = (loanAmount * monthlyInterestRate * Math.pow(1 + monthlyInterestRate, loanTenureMonths)) /
        (Math.pow(1 + monthlyInterestRate, loanTenureMonths) - 1);

    // Display the calculated EMI
    document.getElementById('emiResult').innerText = emi.toFixed(2);

    // Total amount to be paid over the entire tenure
    const totalAmountPayable = emi * loanTenureMonths;

    // Interest component
    const totalInterestPayable = totalAmountPayable - loanAmount;

    // Principal component is the loan amount itself
    const principal = loanAmount;

    // Call function to render pie chart with the breakdown
    renderEMIChart(principal, totalInterestPayable);
});

// Function to render pie chart
function renderEMIChart(principal, interest) {
    const ctx = document.getElementById('emiChart').getContext('2d');

    // Destroy any existing chart instance before creating a new one (for repeated calculations)
    if (window.myPieChart) {
        window.myPieChart.destroy();
    }

    // Create new pie chart
    window.myPieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Principal', 'Interest'],
            datasets: [{
                data: [principal, interest],
                backgroundColor: ['#4CAF50', '#FF5733'],  // Green for principal, red for interest
                hoverBackgroundColor: ['#45a049', '#e74c3c']
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                }
            }
        }
    });
}
