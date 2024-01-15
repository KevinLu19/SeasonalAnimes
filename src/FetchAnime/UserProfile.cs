namespace SeasonalAnime;

/*
 This will be the baseline of what to recommend. 
 
 */
public struct UserProfile
{
    float min_score;
    List<string> anime_genre = new List<string>();
    public UserProfile()
    {
        min_score = 7.5f;

        // Add more genre if necessary to this list.
        anime_genre = new List<string> { "Adventure", "Action",
            "Comedy", "Fantasy", "Drama", "Mystery", "Drama",
            "Romance", "Fantasy" };
    }

    public float GetMinScore()
    {
        return min_score;
    }

    public List<string> GetUserGenre()
    {
        return anime_genre;
    }
}