namespace Services

open Amazon.CloudWatchLogs
open Amazon.CloudWatchLogs.Model
open System

type LogGroup = { Name: string }
type LogEvent = { Message: string }

module AWSCloudwatch =
    let private client = new AmazonCloudWatchLogsClient()

    let defaultPrefix =
        Environment.GetEnvironmentVariable("DEFAULT_LOGGROUP_PREFIX")

    let getLogGroups =
        let response =
            new DescribeLogGroupsRequest(LogGroupNamePrefix = defaultPrefix)
            |> client.DescribeLogGroupsAsync
            |> Async.AwaitTask
            |> Async.RunSynchronously

        response.LogGroups
        |> Seq.toList
        |> List.map (fun x -> { Name = x.LogGroupName })

    let getLogEvents logGroupName =
        let fiveMinsAgo =
            DateTimeOffset(DateTime.Now.AddMinutes(-5.0))
                .ToUnixTimeMilliseconds()

        let now =
            DateTimeOffset(DateTime.Now)
                .ToUnixTimeMilliseconds()

        let response =
            new FilterLogEventsRequest(LogGroupName = logGroupName, Limit = 30, StartTime = fiveMinsAgo, EndTime = now)
            |> client.FilterLogEventsAsync
            |> Async.AwaitTask
            |> Async.RunSynchronously


        response.Events
        |> Seq.toList
        |> List.map (fun x -> { Message = x.Message })
