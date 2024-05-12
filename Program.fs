//CW1 by Lukas Ogunfeitimi
open System; 
open System.Threading;

// Helper functions for presenting
let Prompt(msg: String) =
    Console.Write(msg)
    Console.ReadLine()
    |> ignore
let Sleep(time: int) =
    Thread.Sleep(time)
    |> ignore


/// <summary>Represents a bank account</summary>
type Account() =
    let mutable balance:float = 0.0
    let mutable number:string = "0"
  
    ///<summary>A string representing the account number, with a custom setter to print the 
    ///an account creation message</summary>
    member this.accountNumber
       with get() = number
       and set(value) = 
            number <- value
            Console.WriteLine("Account #" + number.ToString() + " created.")
      
    ///<summary>A float representing the account's balance</summary>
    member this.Balance
       with get() = balance
       and set(value) = balance <- value

      
    ///<summary>Deposits money into the account. Then displays the balance</summary>
    ///<param name="amount">The amount to deposit.</param>
    member this.Deposit(amount: float) = 
        this.Balance <- this.Balance + amount
        Console.Write("Deposited $" + amount.ToString() + ". ")
        this.DisplayBalance()

      
    ///<summary>Attempts to withdraw money from an account. If the asked amount is 
    ///higher than the balance then the transaction will fail.</summary>
    ///<param name="amount">The asked amount to withdraw.</param>
    member this.Withdraw(amount: float) = 
        if amount > this.Balance then
            Console.WriteLine("Insufficient funds to withdraw $" + amount.ToString() + "\n")
        else 
            this.Balance <- this.Balance - amount
            Console.Write("Withdrawn $" + amount.ToString() + ". ")
            this.DisplayBalance()
        |> ignore

    ///<summary>Prints a message based on the account balance</summary>
    member this.CheckAccount() =
        Console.Write("Account #" + this.accountNumber + " ")
        match this.Balance with
            | b when b < 10 -> Console.WriteLine("Balance is low")
            | b when b >= 10 && b <= 100  -> Console.WriteLine("Balance is OK")
            | _ -> Console.WriteLine("Balance is high")
    
    ///<summary>Prints the account number and the balance</summary>
    member this.DisplayBalance() = Console.WriteLine("Account #" + this.accountNumber.ToString() + " balance is: $" + this.Balance.ToString() + "\n")


(* 
Task 1 (10%)
Define an F# type named Account that contains an accountNumber (string) and balance (float) field.
The type includes methods to Withdrawal and Deposit money into the account along with a Print member 
that displays the field values on a single line within the console. 
If the withdrawal amount is greater than the account balance then the transaction must be cancelled 
and a suitable message displayed. Test the type thoroughly.
*)
Prompt("Start Task 1")

let account1 = new Account(accountNumber="34908")
Console.WriteLine()
Sleep(250)
account1.Deposit(1000) // Should work
Sleep(250)
account1.Withdraw(1000) // Should work
Sleep(250)
account1.Withdraw(500) // Shouldn't work


(*
Task 2 (10%)
Define a function named CheckAccount that implements pattern matching to display an appropriate message (see table below) depending upon the current balance of an account.
Balance    Message
< 10.0    Balance is low
>= 10.0 and <= 100.0    Balance is OK
>100.0    Balance is high
Test the function by creating six accounts with the following values
Account Number    Balance
0001    0.0
0002    51.0
0003    5.0
*)
Prompt("Start Task 2")
let accounts = [
    new Account(accountNumber="0001",Balance=0)
    new Account(accountNumber="0002",Balance=51)
    new Account(accountNumber="0003",Balance=5)
    new Account(accountNumber="0004",Balance=120)
    new Account(accountNumber="0005",Balance=6)
    new Account(accountNumber="0006",Balance=67)
    ]
Sleep(250)

// Loop through the 'accounts' list and call the CheckAccount() 
// method from the 'account' type
for account in accounts do
    account.CheckAccount()
    Sleep(75)

Console.WriteLine()

(*
Task 3 (10%)
 Create a list named accounts that holds the six accounts defined in task 4. Using the list create two sequences:
1.    That holds the accounts with a balance greater or equal to zero and less than 50
2.    That holds the accounts with a balance of 50 or more
*)
Prompt("Start Task 3")

//Loop through the 'account' list and 
//make a condition that checks the 'Balance'
//property for a certain amount
let brokie = seq { for a in accounts do if a.Balance >= 0.0 && a.Balance < 50.0 then yield a }
let rich = seq { for a in accounts do if a.Balance >= 50.0 then yield a }


// We must specifically declare the parameter type as we are
// going to access their properties within the loop.
///<summary>Prints the bank account number alongside its balance</summary>
///<param name="accounts">A sequence that contains the 'Account' type.</param>
///<param name="typeAccount">The type of account based on its balance (broke or rich account).</param>
let DisplayAccounts (accounts: Account seq, typeAccount: string) = 
    for a in accounts do
        Console.WriteLine("Account #" + a.accountNumber + " is a " + typeAccount + " account with a balance of $" + a.Balance.ToString())
        Sleep(50)

DisplayAccounts(brokie, "brokie")
Sleep(100)
DisplayAccounts(rich, "rich")

Console.WriteLine()
(*
Task 4 (20%)
Use the record below for a seat booking system and generates a list of 10 tickets for seats which have yet to be allocated to a customer.
type Ticket = {seat:int; customer:string}

let mutable tickets = [for n in 1..10 -> {Ticket.seat = n; Ticket.customer = ""}]

Define two functions:
1.    DisplayTickets function that iterates through the list displaying both field values.
2.    BookSeat function that requests the name of a customer and a seat to a new customer. 
To test this, create two threads that invoke BookSeat and implement locking within BookSeat to avoid a race condition. 
*)
Prompt("Start Task 4 (without lock)")
type Ticket = {seat:int; mutable customer:string}

//Invoke the 'Ticket' type 10 times giving. Assigning the index
//of the loop to the 'seat' property. 
let mutable tickets = [for n in 1..10 -> {Ticket.seat = n; Ticket.customer = ""}]


///<summary>Display each ticket with their seat number 
///and the customer assigned to it.</summary>
///<param name="ticketInfo">A list of the 'Ticket' type</param>
let DisplayTickets (ticketInfo: Ticket list) = 
    for ticket in ticketInfo do
        //Temporary variable for an apprioprate message if the
        //seat isn't assigned to anyoned
        let customer = if ticket.customer <> "" then ticket.customer else "no one"
        Console.WriteLine("Seat #: " + ticket.seat.ToString() + " is assigned to " + customer)
        Sleep(40)

///<summary>Attempt to book a seat for a customer. If the requested
///seat is occupied then reject the request</summary>
///<param name="cusotomerName">The customers name</param>
///<param name="seatNumber">The seat number the customer is requesting</param>
let BookSeat (customerName: string, seatNumber: int)=

    //Loop through the 'Ticket' list until
    //the seat number matches the requested seat number from the customer
    //then check if the seat number is occupied
    //if it is, then reject the request.
    //if it isn't, then assign the seat and end the function
    for ticket in tickets do
        if seatNumber = ticket.seat then
            if ticket.customer = "" then
                ticket.customer <- customerName
                Console.WriteLine("Seat #: " + ticket.seat.ToString() + " has been booked for " + ticket.customer)
                Thread.Sleep(1000) // For testing
            else 
                Console.WriteLine("Seat #: " + ticket.seat.ToString() + " has already been booked by " + ticket.customer)
            |> ignore


// Without using Lock
let StartBooking (customerName:string, seatNumber:int) =
    BookSeat(customerName, seatNumber) 

// Using Lock
let StartBooking2 (customerName:string, seatNumber:int) =
    lock tickets (fun () -> BookSeat(customerName, seatNumber))   

let thread1 = new Thread(fun () -> StartBooking("John", 3)) // Booked
let thread2 = new Thread(fun () -> StartBooking("Lukas", 3)) // Booked

//Start() will tell the program to start scheduling the thread for execution
//Join() will block the calling thread from running until the thread terminates
//in this case the calling thread is our main program. If we wouldn't Join() the thread
//it will still execute but the code after it will also execute immediately
thread1.Start() 
thread2.Start()

thread1.Join()
thread2.Join()

Prompt("Start Lock")

let threadLock1 = new Thread(fun () -> StartBooking2("Jake", 5)) // Booked
let threadLock2 = new Thread(fun () -> StartBooking2("Tom", 5)) // Already booked

threadLock1.Start()
threadLock2.Start()

threadLock1.Join()
threadLock2.Join()

Console.WriteLine()

Prompt("View all Tickets")

DisplayTickets(tickets)

Prompt("End program")



