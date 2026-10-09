# Youtube Database Design

### With Just Likes Count
<div align = "center">
  <img width="600" alt="image" src="https://github.com/user-attachments/assets/8c3aad57-3054-4b51-9dbe-a1319d3b139a" />
</div>

<br>


### With Different reactions 'Like', 'Dislike' etc
<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/9fab3fba-83fd-40ce-8670-00947668658c" />
</div>


<br>

---

<br>

# YouTube SQL Practice - Part 1 (Questions 1–20)

**Some Imp**
1. [Channals Subscribed by Alice](#2-which-channels-does-alice-subscribe-to)
2.  [Mutual Subscription](#53-find-channels-that-both-subscribed-to-each-other-mutual-subscription)
3.  [Top 5 Channal with highest subs](#4-top-5-channels-with-the-highest-subscribers)
4.  [Channals with no subs](#6-find-channels-with-no-subscribers)
5.  [Channals with no uploaded video](#7-users-with-no-uploaded-videos)

<br>

# 1. List all Channels

### SQL

```sql
SELECT *
FROM Channel_Profile;
```

### Logic

Retrieve every record from the `Channel_Profile` table.

<br>

# 2. Which channels does Alice subscribe to?

### SQL

```sql
SELECT C.Name
FROM Subscription S
INNER JOIN Channel_Profile C
    ON S.subscribed_to = C.Id
WHERE S.subscribed_by = 101;      -- Alice's ChannelId
```
```sql
SELECT
    C2.Name AS SubscribedChannel
FROM Subscription S

INNER JOIN Channel_Profile C1
    ON S.subscribed_by = C1.Id

INNER JOIN Channel_Profile C2
    ON S.subscribed_to = C2.Id

WHERE C1.Name = 'Alice';
```

### Logic

Find Alice's subscriptions and join with `Channel_Profile` to display channel names.

<br>

# 3. How many subscribers does each channel have?

### SQL

```sql
SELECT
    C.Name,
    COUNT(S.subscribed_to) AS TotalSubscribers
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_to
GROUP BY
    C.Id,
    C.Name;
```

### Logic

Group subscriptions by the channel being subscribed to and count them.

<br>

# 4. Top 5 channels with the highest subscribers

### SQL

```sql
SELECT TOP 5
    C.Name,
    COUNT(S.subscribed_to) AS TotalSubscribers
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_to
GROUP BY
    C.Id,
    C.Name
ORDER BY
    TotalSubscribers DESC;
```

### Logic

Count subscribers per channel, sort descending and return the Top 5.

<br>

# 5. Which channels have more than 1M subscribers?

### SQL

```sql
SELECT
    C.Name,
    COUNT(*) AS TotalSubscribers
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_to
GROUP BY
    C.Id,
    C.Name
HAVING COUNT(*) > 1000000;
```

### Logic

Group subscriptions by channel and filter using `HAVING`.

<br>

# 6. Find channels with no subscribers

### SQL

```sql
SELECT
    C.Name
FROM Channel_Profile C
LEFT JOIN Subscription S
    ON C.Id = S.subscribed_to
WHERE S.subscribed_to IS NULL;
```

### Logic

LEFT JOIN keeps every channel. If no matching subscription exists, the Subscription columns become NULL.

<br>

# 7. Users with no uploaded videos

### SQL

```sql
SELECT
    C.Name
FROM Channel_Profile C
LEFT JOIN Video V
    ON C.Id = V.ChannelId
WHERE V.ChannelId IS NULL;
```

### Logic

Channels having no matching row in the Video table have never uploaded.

<br>

# 8. Videos uploaded by a channel

### SQL

```sql
SELECT *
FROM Video
WHERE ChannelId = 101;
```

### Logic

Filter videos belonging to a particular channel.

<br>

# 9. Latest video uploaded by a channel

### SQL

```sql
SELECT TOP 1 *
FROM Video
WHERE ChannelId = 101
ORDER BY UploadDate DESC;
```

### Logic

Sort videos by upload date in descending order and return the latest one.

<br>

# 10. Most viewed video

### SQL

```sql
SELECT TOP 1 *
FROM Video
ORDER BY Views DESC;
```

### Logic

Sort by views in descending order and fetch the first row.

<br>

# 11. Top 10 most viewed videos

### SQL

```sql
SELECT TOP 10 *
FROM Video
ORDER BY Views DESC;
```

### Logic

Sort videos by views and return the first 10.

<br>

# 12. Videos uploaded today

### SQL

```sql
SELECT *
FROM Video
WHERE CAST(UploadDate AS DATE) = CAST(GETDATE() AS DATE);
```

### Logic

Compare only the date portion of `UploadDate` with today's date.

<br>

# 13. Videos longer than 20 minutes

### SQL

```sql
SELECT *
FROM Video
WHERE Duration > 20;
```

### Logic

Filter videos whose duration exceeds 20 minutes.

<br>

# 14. Who liked a video?

### SQL

```sql
SELECT
    C.Name
FROM Video_Reaction VR
INNER JOIN Channel_Profile C
    ON VR.ChannelId = C.Id
WHERE
    VR.VideoId = 101
    AND VR.ReactionType = 'Like';
```

### Logic

Join reactions with channels and filter only Likes for a particular video.

<br>

# 15. Which videos did Bob like?

### SQL

```sql
SELECT
    V.Title
FROM Video_Reaction VR
INNER JOIN Video V
    ON VR.VideoId = V.Id
WHERE
    VR.ChannelId = 101
    AND VR.ReactionType = 'Like';
```

### Logic

Filter Bob's reactions and display the corresponding video titles.

<br>

# 16. Display likes and dislikes on every video

### SQL

```sql
SELECT
    V.Title,
    SUM(CASE WHEN VR.ReactionType = 'Like' THEN 1 ELSE 0 END) AS Likes,
    SUM(CASE WHEN VR.ReactionType = 'Dislike' THEN 1 ELSE 0 END) AS Dislikes
FROM Video V
LEFT JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
GROUP BY
    V.Id,
    V.Title;
```

### Logic

Use conditional aggregation to count Likes and Dislikes separately for each video.

<br>

# 17. How many likes does each video have?

### SQL

```sql
SELECT
    V.Title,
    COUNT(*) AS Likes
FROM Video V
INNER JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
WHERE VR.ReactionType = 'Like'
GROUP BY
    V.Id,
    V.Title;
```

### Logic

Filter only Likes, then group by video.

<br>

# 18. Videos with zero likes

### SQL

```sql
SELECT
    V.Title
FROM Video V
LEFT JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
    AND VR.ReactionType = 'Like'
WHERE VR.VideoId IS NULL;
```

### Logic

Join only Like reactions. NULL indicates the video has never received a Like.

<br>

# 19. Users who liked their own videos

### SQL

```sql
SELECT
    C.Name,
    V.Title
FROM Video V
INNER JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
INNER JOIN Channel_Profile C
    ON C.Id = V.ChannelId
WHERE
    VR.ChannelId = V.ChannelId
    AND VR.ReactionType = 'Like';
```

### Logic

Compare the uploader's `ChannelId` with the reacting user's `ChannelId`.

<br>

# 20. Show all comments on a video

### SQL

```sql
SELECT
    CommentText
FROM Video_Comments
WHERE VideoId = 101;
```

### Logic

Filter the comments table using the required `VideoId`.

<br>

---

<br>

# YouTube SQL Practice - Part 2 (Questions 21–40)

<br>

# 21. Comments with commenter names

### SQL

```sql
SELECT
    C.Name,
    VC.CommentText
FROM Video_Comments VC
INNER JOIN Channel_Profile C
    ON VC.ChannelId = C.Id
WHERE VC.VideoId = 101;
```

### Logic

Join the comments table with the channel table to display the comment along with the commenter's name.

<br>

# 22. Number of comments per video

### SQL

```sql
SELECT
    V.Title,
    COUNT(VC.Id) AS TotalComments
FROM Video V
LEFT JOIN Video_Comments VC
    ON V.Id = VC.VideoId
GROUP BY
    V.Id,
    V.Title;
```

### Logic

Group comments by video and count them.

<br>

# 23. Most commented video

### SQL

```sql
SELECT TOP 1
    V.Title,
    COUNT(VC.Id) AS TotalComments
FROM Video V
INNER JOIN Video_Comments VC
    ON V.Id = VC.VideoId
GROUP BY
    V.Id,
    V.Title
ORDER BY
    TotalComments DESC;
```

### Logic

Count comments for each video and return the highest.

<br>

# 24. Users who commented the most

### SQL

```sql
SELECT TOP 1
    C.Name,
    COUNT(VC.Id) AS TotalComments
FROM Channel_Profile C
INNER JOIN Video_Comments VC
    ON C.Id = VC.ChannelId
GROUP BY
    C.Id,
    C.Name
ORDER BY
    TotalComments DESC;
```

### Logic

Group comments by user and return the user with the highest comment count.

<br>

# 25. Videos in a playlist

### SQL

```sql
SELECT
    V.Title
FROM Playlist_Video PV
INNER JOIN Video V
    ON PV.VideoId = V.Id
WHERE PV.PlaylistId = 101;
```

### Logic

Join the bridge table with Videos and filter by Playlist.

<br>

# 26. Number of videos in each playlist

### SQL

```sql
SELECT
    P.Name,
    COUNT(PV.VideoId) AS TotalVideos
FROM Playlist P
LEFT JOIN Playlist_Video PV
    ON P.Id = PV.PlaylistId
GROUP BY
    P.Id,
    P.Name;
```

### Logic

Count how many videos belong to every playlist.

<br>

# 27. Playlists created by Alice

### SQL

```sql
SELECT
    Name
FROM Playlist
WHERE ChannelId = 101;
```

### Logic

Filter playlists created by Alice's ChannelId.

<br>

# 28. Videos watched by Bob

### SQL

```sql
SELECT
    V.Title
FROM Watch_History WH
INNER JOIN Video V
    ON WH.VideoId = V.Id
WHERE WH.ChannelId = 101;
```

### Logic

Join Watch History with Videos and filter Bob.

<br>

# 29. Most watched video

### SQL

```sql
SELECT TOP 1
    V.Title,
    COUNT(*) AS WatchCount
FROM Watch_History WH
INNER JOIN Video V
    ON WH.VideoId = V.Id
GROUP BY
    V.Id,
    V.Title
ORDER BY
    WatchCount DESC;
```

### Logic

Count watches for each video and return the highest.

<br>

# 30. User's recently watched videos

### SQL

```sql
SELECT
    V.Title
FROM Watch_History WH
INNER JOIN Video V
    ON WH.VideoId = V.Id
WHERE WH.ChannelId = 101
ORDER BY WH.WatchedAt DESC;
```

### Logic

Sort the user's watch history by watch date in descending order.

<br>

# 31. Total watch history count per user

### SQL

```sql
SELECT
    C.Name,
    COUNT(*) AS TotalWatched
FROM Channel_Profile C
INNER JOIN Watch_History WH
    ON C.Id = WH.ChannelId
GROUP BY
    C.Id,
    C.Name;
```

### Logic

Group watch history by user and count the total videos watched.

<br>

# 32. Find videos with no comments

### SQL

```sql
SELECT
    V.Title
FROM Video V
LEFT JOIN Video_Comments VC
    ON V.Id = VC.VideoId
WHERE VC.VideoId IS NULL;
```

### Logic

LEFT JOIN keeps every video. NULL means no comments exist.

<br>

# 33. Find videos with no reactions

### SQL

```sql
SELECT
    V.Title
FROM Video V
LEFT JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
WHERE VR.VideoId IS NULL;
```

### Logic

Videos without matching reactions have NULL on the reaction side.

<br>

# 34. Find playlists containing more than 20 videos

### SQL

```sql
SELECT
    P.Name,
    COUNT(PV.VideoId) AS TotalVideos
FROM Playlist P
INNER JOIN Playlist_Video PV
    ON P.Id = PV.PlaylistId
GROUP BY
    P.Id,
    P.Name
HAVING COUNT(PV.VideoId) > 20;
```

### Logic

Group playlist videos and filter using HAVING.

<br>

# 35. Find channels that never created a playlist

### SQL

```sql
SELECT
    C.Name
FROM Channel_Profile C
LEFT JOIN Playlist P
    ON C.Id = P.ChannelId
WHERE P.ChannelId IS NULL;
```

### Logic

Channels with NULL Playlist rows have never created one.

<br>

# 36. Find the average views per channel

### SQL

```sql
SELECT
    C.Name,
    AVG(V.Views) AS AverageViews
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
GROUP BY
    C.Id,
    C.Name;
```

### Logic

Group videos by channel and calculate the average views.

<br>

# 37. Find the total views earned by each channel

### SQL

```sql
SELECT
    C.Name,
    SUM(V.Views) AS TotalViews
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
GROUP BY
    C.Id,
    C.Name;
```

### Logic

Sum all video views belonging to each channel.

<br>

# 38. Find the channel with the most uploaded videos

### SQL

```sql
SELECT TOP 1
    C.Name,
    COUNT(V.Id) AS TotalVideos
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
GROUP BY
    C.Id,
    C.Name
ORDER BY
    TotalVideos DESC;
```

### Logic

Count uploads for every channel and return the highest.

<br>

# 39. Find videos that have more likes than dislikes

### SQL

```sql
SELECT
    V.Title,
    SUM(CASE WHEN VR.ReactionType = 'Like' THEN 1 ELSE 0 END) AS Likes,
    SUM(CASE WHEN VR.ReactionType = 'Dislike' THEN 1 ELSE 0 END) AS Dislikes
FROM Video V
LEFT JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
GROUP BY
    V.Id,
    V.Title
HAVING
    SUM(CASE WHEN VR.ReactionType = 'Like' THEN 1 ELSE 0 END) >
    SUM(CASE WHEN VR.ReactionType = 'Dislike' THEN 1 ELSE 0 END);
```

### Logic

Calculate both Like and Dislike counts and compare them using HAVING.

<br>

# 40. Find channels that subscribed to more than 10 channels

### SQL

```sql
SELECT
    C.Name,
    COUNT(S.subscribed_to) AS TotalSubscriptions
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_by
GROUP BY
    C.Id,
    C.Name
HAVING
    COUNT(S.subscribed_to) > 10;
```

### Logic

Group subscriptions by the subscriber (`subscribed_by`) and count how many channels each user follows.

<br>

---

<br>

# YouTube SQL Practice - Part 3 (Questions 41–53)

<br>

# 41. Find videos appearing in multiple playlists

### SQL

```sql
SELECT
    V.Title,
    COUNT(PV.PlaylistId) AS PlaylistCount
FROM Video V
INNER JOIN Playlist_Video PV
    ON V.Id = PV.VideoId
GROUP BY
    V.Id,
    V.Title
HAVING COUNT(PV.PlaylistId) > 1;
```

### Logic

Group videos by `VideoId` and count how many playlists each video belongs to.

<br>

# 42. Find the average video duration per channel

### SQL

```sql
SELECT
    C.Name,
    AVG(V.Duration) AS AverageDuration
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
GROUP BY
    C.Id,
    C.Name;
```

### Logic

Group videos by channel and calculate the average duration.

<br>

# 43. Find the top 3 most viewed videos of each channel (Window Function)

### SQL

```sql
WITH RankedVideos AS
(
    SELECT
        V.Title,
        V.Views,
        C.Name AS ChannelName,
        ROW_NUMBER() OVER
        (
            PARTITION BY V.ChannelId
            ORDER BY V.Views DESC
        ) AS RankNo
    FROM Video V
    INNER JOIN Channel_Profile C
        ON V.ChannelId = C.Id
)

SELECT *
FROM RankedVideos
WHERE RankNo <= 3;
```

### Logic

Partition videos by channel, rank them by views, and return the top 3 from every channel.

<br>

# 44. Find the most active commenter

### SQL

```sql
SELECT TOP 1
    C.Name,
    COUNT(VC.Id) AS TotalComments
FROM Channel_Profile C
INNER JOIN Video_Comments VC
    ON C.Id = VC.ChannelId
GROUP BY
    C.Id,
    C.Name
ORDER BY
    TotalComments DESC;
```

### Logic

Count comments for every user and return the highest.

<br>

# 45. Find channels that uploaded videos but received no likes

### SQL

```sql
SELECT DISTINCT
    C.Name
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
LEFT JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
    AND VR.ReactionType = 'Like'
WHERE VR.VideoId IS NULL;
```

### Logic

Join videos with only Like reactions. NULL means that uploaded video never received a Like.

<br>

# 46. Rank videos by views within each channel

### SQL

```sql
SELECT
    C.Name AS ChannelName,
    V.Title,
    V.Views,
    RANK() OVER
    (
        PARTITION BY V.ChannelId
        ORDER BY V.Views DESC
    ) AS VideoRank
FROM Video V
INNER JOIN Channel_Profile C
    ON V.ChannelId = C.Id;
```

### Logic

Rank every video within its own channel based on views.

<br>

# 47. Find the second most viewed video overall

### SQL

```sql
WITH RankedVideos AS
(
    SELECT
        Title,
        Views,
        DENSE_RANK() OVER
        (
            ORDER BY Views DESC
        ) AS RankNo
    FROM Video
)

SELECT *
FROM RankedVideos
WHERE RankNo = 2;
```

### Logic

Rank all videos by views and return those having Rank = 2.

<br>

# 48. Find the latest uploaded video for every channel

### SQL

```sql
WITH LatestVideos AS
(
    SELECT
        C.Name AS ChannelName,
        V.Title,
        V.UploadDate,
        ROW_NUMBER() OVER
        (
            PARTITION BY V.ChannelId
            ORDER BY V.UploadDate DESC
        ) AS RowNo
    FROM Video V
    INNER JOIN Channel_Profile C
        ON V.ChannelId = C.Id
)

SELECT *
FROM LatestVideos
WHERE RowNo = 1;
```

### Logic

Within every channel, assign row number by latest upload date and select the first row.

<br>

# 49. Find channels whose subscriber count is greater than their upload count

### SQL

```sql
WITH SubscriberCount AS
(
    SELECT
        subscribed_to AS ChannelId,
        COUNT(*) AS Subscribers
    FROM Subscription
    GROUP BY subscribed_to
),
UploadCount AS
(
    SELECT
        ChannelId,
        COUNT(*) AS Uploads
    FROM Video
    GROUP BY ChannelId
)

SELECT
    C.Name,
    SC.Subscribers,
    UC.Uploads
FROM Channel_Profile C
INNER JOIN SubscriberCount SC
    ON C.Id = SC.ChannelId
INNER JOIN UploadCount UC
    ON C.Id = UC.ChannelId
WHERE SC.Subscribers > UC.Uploads;
```

### Logic

Calculate subscriber count and upload count separately, then compare them.

<br>

# 50. Find videos liked by subscribers of the uploader

### SQL

```sql
SELECT DISTINCT
    V.Title
FROM Video V
INNER JOIN Subscription S
    ON V.ChannelId = S.subscribed_to
INNER JOIN Video_Reaction VR
    ON V.Id = VR.VideoId
    AND VR.ChannelId = S.subscribed_by
WHERE VR.ReactionType = 'Like';
```

### Logic

Find subscribers of the uploader, then check whether those subscribers liked the uploader's videos.

<br>

# 51. Find channels that have uploaded videos in every month of a year

### SQL

```sql
SELECT
    C.Name
FROM Channel_Profile C
INNER JOIN Video V
    ON C.Id = V.ChannelId
WHERE YEAR(V.UploadDate) = 2025
GROUP BY
    C.Id,
    C.Name
HAVING COUNT(DISTINCT MONTH(V.UploadDate)) = 12;
```

### Logic

Count distinct upload months and keep channels having uploads in all 12 months.

<br>

# 52. Find the most watched video by each user

### SQL

```sql
WITH WatchCounts AS
(
    SELECT
        WH.ChannelId,
        V.Title,
        COUNT(*) AS WatchCount,
        ROW_NUMBER() OVER
        (
            PARTITION BY WH.ChannelId
            ORDER BY COUNT(*) DESC
        ) AS RowNo
    FROM Watch_History WH
    INNER JOIN Video V
        ON WH.VideoId = V.Id
    GROUP BY
        WH.ChannelId,
        V.Title
)

SELECT *
FROM WatchCounts
WHERE RowNo = 1;
```

### Logic

Count how many times each user watched every video, rank them within each user, and return the highest.

<br>

# 53. Find channels that both subscribed to each other (Mutual Subscription)

### SQL

```sql
SELECT
    C1.Name AS Channel1,
    C2.Name AS Channel2
FROM Subscription S1
INNER JOIN Subscription S2
    ON S1.subscribed_by = S2.subscribed_to
    AND S1.subscribed_to = S2.subscribed_by
INNER JOIN Channel_Profile C1
    ON S1.subscribed_by = C1.Id
INNER JOIN Channel_Profile C2
    ON S1.subscribed_to = C2.Id;
WHERE S1.subscribed_by < S1.subscribed_to;
```

### Logic

Self-join the `Subscription` table to find pairs where both channels subscribe to each other.

| Id | subscribed_by | subscribed_to |
| -: | ------------: | ------------: |
|  1 |            10 |            20 |
|  2 |            20 |            10 |

| Id | Name  |
| -: | ----- |
| 10 | Alice |
| 20 | Bob   |

after first join (2 iterations)
| S1.Id | S1.by | S1.to | S2.Id | S2.by | S2.to |
| ----: | ----: | ----: | ----: | ----: | ----: |
|     1 |    10 |    20 |     2 |    20 |    10 |
|     2 |    20 |    10 |     1 |    10 |    20 |

| S1.Id | S1.by | S1.to | S2.Id | S2.by | S2.to | C1.Name | C2.Name |
| ----: | ----: | ----: | ----: | ----: | ----: | ----: | ----: |
|     1 |    10 |    20 |     2 |    20 |    10 | Alice | Bob |
|     2 |    20 |    10 |     1 |    10 |    20 | Bob | Alice |

Op 
| Name1  | Name 2 |
 | ----- | -------|
  | Alice | Bob| 


<br>

---

<br>
