using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeasonalAnime.Gil;

/// <summary>
/// Class handles the daily command. Supply 200 Gil when it's been 24 hours or more past the last call. 
/// Gil coin shop is still under construction. 
/// Will need to get a database to keep a record.
/// </summary>
internal class Daily
{
    private readonly int _gil_amount;
    private DateTime current_date_time;
    public Daily()
    {
        _gil_amount = 200;
    }

	/// <summary>
	/// Function checks if it has been 24 hour since the user last called the command.
    /// If yes, give gil otherwise reject.
	/// </summary>
	/// <param name="called_datetime"> => User called command's time</param>
	public bool CheckTime(DateTime called_datetime)
    { 
        /*
         Result < 0 : current_date_time earlier than called_datetime
         Result == 0 : Both time are the same
         Result > 0 : currente_date_time later than called_datetime
         */
        var result = DateTime.Compare(current_date_time, called_datetime);
    
        if (result < 0)
        {
            // called_datetime is bigger t han currentdate (not 24 hours yet). Can't give Gil
            return false;
        }
        else if (result == 0)
        {
            // Both datetime are equal. Able to give Gil
            return true;
        }
        else
        {
            // Current_date_time bigger than called_datetime. Can give Gil
            return true;
        }

    }

    public int GiveGil()
    {
        return _gil_amount;
    }
}
