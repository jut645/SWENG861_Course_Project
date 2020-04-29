# FlightPrices

The FlightPrices solution is a solution for the SWENG861 Spring 2020 Course Project.
This project is based on Option 2: allowing an end-user to view real-time flight prices.
To run the FlightPrices solution, two projects within the solution are necessary. These projects
are the FlightPrices.WebAPI and FlightPrices.WebApp projects; these projects are described below.


# FlightPrices.WebApp

The FlightPrices.WebApp project is the user-facing project of the FlightPrices solution.
This project contains all of the views, forms, and user-input validation rules that will 
faciliate the user interacting with the flight price search service. The FlightPrices.WebApp
project uses HttpClients to communicate with the FlightPrices.WebAPI project. The user provides
the expected input through the FlightPrices.WebApp project, then the project requests the flight
price data from the FlightPrices.WebAPI project, and finally the FlightPrices.WebApp project provides
the output to the user.


# FlightPrices.WebAPI

The FlightPrices.WebAPI project handles communication with the third-party API to get the real-time
flight price data. This project exposes a RESTful API and maps requests to and from the third-party
flight price API. In the context of the FlightPrices solution, the FlightPrices.WebApp sends a request to 
the RESTful API offered by the FlightPrices.WebAPI. The FlightPrices.WebAPI then maps the request to
the format expected by the third-party API and makes the request to it. After recieving the response from
the third-party API, the FlightPrices.WebAPI maps the response data and sends it back to the FlightPrices.WebApp.  

# Inputs
The expected inputs into the system are:

* The name of an existing airport for arrival, different from origin (e.g. Jacksonville International Airport).
* The name of an existing airport for origin, different from arrival (e.g. John F. Kennedy International Airport).
* The date to depart from the origin airport (e.g. 05-14-2020).
* The date to return to the origin airport (e.g. 05-18-2020).
* Whether the trip is round-trip or one-way (e.g. Yes for round-trip, No for One-Way).

# Outputs
The expected outputs from the system are:

* The flight number of the departure airline (e.g. 1078).
* The flight airline name of the departure airline (e.g. Spirit)
* The time of the departure from the origin airport in EST (e.g. 01:41 PM)
* The time of the arrival to the destination airport in EST (e.g. 05:30 PM)
  If the flight is overnight, indicate the date of arrival as well (e.g. 05:30 PM (05/18))
* The number of stops on the trip from the origin airport to the destination airport (e.g. 1)

if round trip:
* The flight number of the return airline (e.g. 1078).
* The flight airline name of the return airline (e.g. Spirit)
* The time of the departure from the destination airport in EST (e.g. 01:41 PM)
* The time of the arrival to the origin airport in EST. 
  If the flight is overnight, indicate the date of arrival as well (e.g. 05:30 PM (05/18))
* The number of stops on the trip from the destination airport to the origin airport (e.g. 1)

* The cost in American USD (e.g. $82)

# Third-Party API

The FlightPrices solution relies on a third-party API for real-time flight data. The FlightPrices.WebAPI
project contains a client for interacting with the TripAdvisor API through RapidAPI.

### Prerequisites

Download the zip file containing the FlightPrices solution and unzip it to a location on your local
machine.

The simplest way to run this solution will be by using Microsoft's Visual Studio.

Please visit https://visualstudio.microsoft.com/vs/ and download the latest 
version of Visual Studio Community Edition.

When the download is finished, please go through the installation process.

A SQL Server database will be used to store the Airport data. 
See https://www.microsoft.com/en-us/download/details.aspx?id=55994 for a download link for 
SQL Server Express 2017. Download and run the installer.

To create the SQL Server Database used for Airport information, we will use 
SQL Server Management Studio.

Please visit https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15
and download the latest version of SQL Server Management Studio. Once downloaded,
run the installer.

### Installing

To run the FlightPrices solution, do the following:

**1. Create the FlightPrices SQL Server Database**

1.1 Open SQL Server Management Studio

1.2 Click "Connect" followed by "Database Engine".

1.3 For the "Server Name" dropdown, locate your SQL Express installation.
    For example, mine was "DESKTOP-DAM9VI8\SQLEXPRESS".

1.4 Select "Windows Authentication" for the Authentication dropdown and 
    click "Connect".

1.5 Right click on "Database" and select "Import Data-Tier Application..."

1.6 Select "Next", then "Import from local disk", and the "Browse".

1.7 Find the unzipped project solution on your local disk. Go to 
    FlightPrices/FlightPrices.Skyscanner.WebAPI/Resources and select
    open "FlightPricesDB.bacpac" and then click "Next".

1.8 Change the "New database name" field from "FlightPricesDB" to "FlightPrices".

1.9 Continue clicking next until you can click "Finish". Allow the import to complete.

**2. Open the solution in Visual Studio**

2.1 Open Visual Studio

2.2 Click the "Open a project or solution" option.

2.3 Browse to where you unzipped the FlightPrices solution.

2.4 Open the FlightPrices.sln file

2.5 If necessary, right click on the FlightPrices Solution and select "Restore Nugat package"
    to download dependencies.

2.6 Right click on the FlightPrices Solution file, select "Select Startup Projects".

2.7 Select "Multiple startup project", and then set the FlightPrices.WebApp and FlightPrices.WebAPI
   Actions to Start. Click Apply.

2.8 Either click the "Start" button at the top of Visual Studio with the green arrow, or 
    hit the key combiation "CONTROL + F5" to run without debugging.

## Running the tests

The run the tests for the solution:

1. Ensure the Nuget package "XUnit" and "Moq" are installed for the FlightPrices.Tests project.

2. Open the solution in Visual Studio.

3. Right click on FlightPrices.Tests.

4. Click "Run Tests".

## Built With

* [.NET Core](https://dotnet.microsoft.com/download) - The web framework used
* [Nuget](https://www.nuget.org/) - Dependency Management

## Authors

* **Jake Taylor** - *Primary Author*
