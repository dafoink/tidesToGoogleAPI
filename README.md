# tidesToGoogleAPI
This project requires dotNetCore.  You can download dotNetCore from Microsoft (https://dotnet.microsoft.com/download/dotnet-core)
This should run on any machine that can run the .NET Core runtime.  I have tested on Windows 10 and Ubuntu (Linux)

Usage: ./Tides.exe -beginDate <MM/dd/yyyy> -endDate <MM/dd/yyyy> -station <stationID> -fileName <./filename.csv>

If you dont enter a filename, the data will display on the console.

You can get a station ID near you by going to NOAA's site: https://tidesandcurrents.noaa.gov/map/index.html

Example, to find the tides at the station near mine for the entire year of 2021, I would enter:

./Tides.exe -beginDate 01/01/2021 -endDate 12/31/2021 -station 8654467 -fileName ./tidesOutputFile.csv

Where:
- beginDate is the first day I want to get the tides (in this case 01/01/2021)
- endDate is the last day I want to get tides (in this case 12/31/2021)
- station is the location for getting the tides.  The station closest to me on Hatteras Island is 8654467 (USCG Station Hatteras, NC)
- fileName is where I want to output the information.  I am saving it in the same folder I am executing Tides.exe and naming the file tidesOutputFile.csv
