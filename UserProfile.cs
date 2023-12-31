
namespace SeasonalAnime;

/*
 This will be the baseline of what to recommend. 
 
 */
public struct UserProfile
{
    float min_score;
    List<String> anime_genre; 
    public UserProfile()
    {
        min_score = 7.5f;

        // Add more genre if necessary to this list.
        anime_genre = new List<String> { "Adventure", "Action", 
            "Comedy", "Fantasy", "Drama", "Mystery", "Drama", 
            "Romance", "Fantasy" };
    }

    public float GetMinScore()
    {
        return min_score;
    }

    public List<String> GetUserGenre()
    {
        return anime_genre;
    }
}