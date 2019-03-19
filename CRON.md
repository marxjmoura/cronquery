# CRON

Cron is a time-based job scheduler. Jobs run periodically at fixed times configured using a six-field expression.

```
┌──── second (0-59)
│  ┌───── minute (0-59)
│  │  ┌───── hour (0-23)
│  │  │  ┌───── day of month (1-31)
│  │  │  │  ┌───── month (1-12)
│  │  │  │  │  ┌───── day of week (0-6, starting on Sunday)
│  │  │  │  │  │
*  *  *  *  *  *
```

| Field        | Allowed values | Special characters          |
|--------------|----------------|-----------------------------|
| Second       | 0-59           | `*` `-` `,` `/`             |
| Minute       | 0-59           | `*` `-` `,` `/`             |
| Hour         | 0-23           | `*` `-` `,` `/`             |
| Day of month | 1-31           | `*` `-` `,` `/` `L` `W`     |
| Month        | 1-12           | `*` `-` `,` `/`             |
| Day of week  | 0-6            | `*` `-` `,` `/` `L` `#`     |

## Special characters

The following table shows the special characters present in cron syntax and their respective meanings.

| Character        | Description                                             | Example |
|------------------|---------------------------------------------------------|---------|
| `*` (all)        | Every time unit                                         | Asterisk in the hour means "every hour" |
| `-` (range)      | Range of values                                         | 1-5, which is equivalent to 1,2,3,4,5 |
| `,` (list)       | List of values                                          | 2,4,6,8 |
| `/` (increment)  | Step values                                             | */6 in the hour means "every 6 hours", which is equivalent to 0,6,12,18 |
| `L` (last)       | Last day of month or last X day of week                 | L means 31 for January or day 28 for February (non-leap years) and 5L means "the last Friday of the month" |
| `W` (weekday)    | Weekday (Monday-Friday) nearest the given day           | If you specify 15W and 15th is a Saturday the trigger fires on Friday the 14th, however for 1W and 1th is a Saturday the trigger fires on Monday the 3rd (because it does not exceed the limit of days of the month), if the 15h is a Sunday the trigger fires on Monday the 16th and if 15th is a weekday the trigger fires that day |
| `#` (occurrence) | The nth day of the month                                | 5#3 means the third Friday of the month |

## Supported combinations

Cron allows the combination of subexpressions to produce a more complex cron expression.
The following list shows the combinations supported by CronQuery:

- `*`: Every time unit.
- `00-00`: Range of values.
- `*/00`: Every time unit skipping a given number of values.
- `00/00`: Skipping a given number of values starting at specific value.
- `00-00/00`: Skipping a given number of values in a given range of values.
- `00,00-00,00/00,00-00/00`: List of values (you can produce variations). List can only contains single numbers, range, increment and range combined with increment.
- `L` (day of month only): Last day of the month (including weekend).
- `LW` (day of month only): Last weekday (Monday-Friday) of the month.
- `L-00` (day of month only): Days (1-31) before the last day of month.
- `00W` (day of month only): Weekday nearest the given day (1-31).
- `0L` (day of week only): Last day of week (0-6) of the month.
- `00#0` (day of week only): The nth (1-5) day of the month.

> Zero means a numeric parameter that you pass to the expression.

## Examples

The following table shows examples of cron expressions and their respective meanings.

| Expression        | Description                                             |
|-------------------|---------------------------------------------------------|
| 0 0 12 * * *      | At 12:00 PM every day                                   |
| 0 0 18 L * *      | At 6:00 PM on the last day of every month               |
| 0 0 18 L-3 * *    | At 6:00 PM 3 days before the last day of the month      |
| 0 0 0 * */6 *     | At 12:00 AM every 6 months                              |
| 0 0 0 5,15,25 * * | At 12:00 AM, on day 5, 15, and 25 of the month          |
| 0 */5 * * * *     | Every 5 minutes                                         |
| 0 0 8-18/2 * * *  | Every 2 hours between 8:00 AM and 6:00 PM               |
| 0 0 10 * * 1#3    | At 10:00 AM on the third Monday of every month          |

> https://crontab.guru is a great tool to help create cron expressions.
