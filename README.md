# SolarmanApiDotNet

## [WIP]

This project aims to simplify integration with the solarman API.

### Current features
- Rest API with endpoint to get plant live data
- Scheduled service (Configurable schedule) that periodically gets the live data and logs warnings when batteries are discharging/grid is offile

### Planned Features
- Send email to configurable addres when some state has been reached
- Send push notification to companion mobile app (mobile app not yet implemented) for custom alerts (Mobile app will register on this server, so no further integration needed)
- Authentication via 3rd party (google, facebook etc.) to secure data
- Full solarman Api integration (will probably not use most of the endpoints, but implementing the requests and responses might be useful for future development)

### Stretch Goals
- Allow mobile app to configure server

### Setting up your appsettings
- Send an email to service@solarmanpv.com requesting api access (this is a conversation, so they will ask you some questions)
- Next you need to set the values in appsettings.json to your info, explained below:
#### appsettings.json explained:
```
 "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "Kestrel": {
        "EndPoints": {
            "Http": {
                "Url": "http://localhost:5555"
            }
        }
    },
    "AllowedHosts": "*",
    "SolarmanApiOptions": {
        "BaseUrl": "https://globalapi.solarmanpv.com" // Where to send network requests to
    },  
  "SolarmanAuthenticationOptions": {
        "issuer": "https://globalapi.solarmanpv.com", // Where to get an auth token from (this is configurable in the case a different identity provider should be used)
        "appSecret": "", // Provided by these guys service@solarmanpv.com 
        "appId": "", // Provided by these guys service@solarmanpv.com
        "email": example@example.com", // The email you use to login here https://home.solarmanpv.com/
        "password": "" // Your password. Passwords longer than 16 characters and containing special characters may not work!
    },
    "CronOptions": [
    {
      "serviceName": "ScheduledGetLiveData", // name of ISheduledservice
      "schedule": "*/10 * * * *", // Normal crons tring, leave as is for every 10 minutes
      "runOnStart": true // runs when the schedule is started
    }
```    

### Running the project
- Open the .sln file in Rider or Visual Studio
- Run the SolarmanApi project
- Open your browser on http://localhost:5555/swagger/index.html
- Execute /api/Solarman/GetCurrentLiveData 


Here is what it looks like if its working correctly
![image](https://user-images.githubusercontent.com/4478381/176170948-37166a03-f49f-4691-9998-a1350af84599.png)

