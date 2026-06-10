(Roadmap) Feature to sync local data to and from cloud. Allow for features like

notis off lengthy training blocks -> should consider rotation
plateaus -> deload
Upload/download of training templates
Syncing across devices.
AI

The how:

sqlite upload -> what happens after a year of use
per obj -> higher $ -- whats happening rn
sqlite-ish state upload - what happens with updated days
bulk JSON -> pipelining fits well - need to consider what happens with cross container aggs

On the app

sync button
event triggered
cron

MANAGING THE DATA

- Containers
1. Workouts
2. Measurements *probably not* -> updated on cron or trigger of new workout item - probably not event/trigger based
3. Sync State (Rollups) -> Think checkpointing - some type of state management
4. Templates 
5. Exercises? ^ could tie in with templates


Sync Flow:

```
POST /sync

Checkpoint where/when/what has been updated -> requires consideration

Start upserting items 

Checkboxing items -> Add on DTO

*maybe* trigger measurements -> could probably be fine on client side  
```

...
