namespace BettingWallet.Core
{
    public static class Constants
    {
        public const string EXIT_MESSAGE = "Thank you for playing! Hope to see you again soon.";
        public const string BET_WIN_MESSAGE = "Congrats - you won ${0:F2}! Your current balance is: ${1:F2}";
        public const string BET_LOSS_MESSAGE = "No luck this time! Your current balance is: ${0:F2}";
        public const string WITHDRAWAL_MESSAGE = "Your withdrawal of ${0:F2} was successful. Your current balance is: ${1:F2}";
        public const string SUBMIT_ACTION_MESSAGE = "Please, submit action";
        public const string DEPOSIT_MESSAGE = "Your deposit of ${0:F2} was successful. Your current balance is ${1:F2}";
        public const string UNSUPPORTED_COMMAND_MESSAGE = $"Invalid command. Supported commands are: {BET}, {WITHDRAW}, {DEPOSIT}, {EXIT}";
        public const string INVALID_ARGUMENT_OPERATION_MESSAGE = "In order to {0} you need to provide amount";
        public const string AMOUNT_OUT_OF_RANGE_MESSAGE = "Bet amount must be between $1 and $10";
        public const string INVALID_NUMBER_OF_PARAMETERS = "Parameters must be between 1 and 2";
        public const string UNEXPECTED_ERROR_MESSAGE = "Something went wrong on our end, please try again! {0}";
        public const string AMOUNT_MUST_BE_POSITIVE_MESSAGE = "Amount must be a positive number!";
        public const string INSUFFICIENT_FUNDS_MESSAGE = "Insufficient funds";
        public const string INVALID_OUTCOME_MESSAGE = "Invalid outcome";

        public const decimal MIN_BETTING_AMOUNT = 1m;
        public const decimal MAX_BETTING_AMOUNT = 10m;

        public const decimal WIN_THRESHOLD = 50m;
        public const decimal UP_TO_TWO_TIMES_WIN_THRESHOLD = 90;

        public const int UP_TO_TWO_TIMES_COEFFICIENT_LOWER_BOUND = 1;
        public const int UP_TO_TWO_TIMES_COEFFICIENT_UPPER_BOUND = 2;
        public const int UP_TO_TEN_TIMES_COEFFICIENT_LOWER_BOUND = 2;
        public const int UP_TO_TEN_TIMES_COEFFICIENT_UPPER_BOUND = 10;

        public const string BET = "bet";
        public const string WITHDRAW = "withdraw";
        public const string EXIT = "exit";
        public const string DEPOSIT = "deposit";
    }
}
