 Feature: Merchant Fees
		 As a MobilePay accountant
		 I want to have an app which will calculate merchant fees
		 So that we would avoid manual calculation

@MOBILEPAY-2
Scenario: Standard transaction percentage fee
Given TransactionPercentageFee is configured to be 'enabled'
And InvoiceFixedFee is configured to be 'disabled'
And '120' DKK transaction is made to 'CIRCLE_K' on '2018-09-02'
And '200' DKK transaction is made to 'TELIA' on '2018-09-04'
And '300' DKK transaction is made to 'CIRCLE_K' on '2018-10-22'
And '150' DKK transaction is made to 'CIRCLE_K' on '2018-10-29'
When fees calculation app is executed
Then the output is equal to 'mobilepay-2.txt' file content

@MOBILEPAY-3
Scenario: Discount for TELIA
Given TransactionPercentageFee is configured to be 'enabled'
And InvoiceFixedFee is configured to be 'disabled'
And 'TELIA' has TransactionPercentageFee discount of '10' percent
And '120' DKK transaction is made to 'TELIA' on '2018-09-02'
And '200' DKK transaction is made to 'TELIA' on '2018-09-04'
And '300' DKK transaction is made to 'TELIA' on '2018-10-22'
And '150' DKK transaction is made to 'TELIA' on '2018-10-29'
When fees calculation app is executed
Then the output is equal to 'mobilepay-3.txt' file content

@MOBILEPAY-4
Scenario: Discount for CIRCLE_K
Given TransactionPercentageFee is configured to be 'enabled'
And InvoiceFixedFee is configured to be 'disabled'
And 'CIRCLE_K' has TransactionPercentageFee discount of '20' percent
And '120' DKK transaction is made to 'CIRCLE_K' on '2018-09-02'
And '200' DKK transaction is made to 'CIRCLE_K' on '2018-09-04'
And '300' DKK transaction is made to 'CIRCLE_K' on '2018-10-22'
And '150' DKK transaction is made to 'CIRCLE_K' on '2018-10-29'
When fees calculation app is executed
Then the output is equal to 'mobilepay-4.txt' file content

@MOBILEPAY-5
Scenario: InvoiceFixedFee for the first transaction of the month
Given TransactionPercentageFee is configured to be 'enabled'
And InvoiceFixedFee is configured to be 'enabled'
And '120' DKK transaction is made to '7-ELEVEN' on '2018-09-02'
And '200' DKK transaction is made to 'NETTO' on '2018-09-04'
And '300' DKK transaction is made to '7-ELEVEN' on '2018-10-22'
And '150' DKK transaction is made to '7-ELEVEN' on '2018-10-29'
When fees calculation app is executed
Then the output is equal to 'mobilepay-5.txt' file content