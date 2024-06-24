using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirestoreService
{
    private readonly FirestoreDb _firestoreDb;

    public FirestoreService(string projectId)
    {
        _firestoreDb = FirestoreDb.Create(projectId);
    }

    public async Task SaveDeviceTokenAsync(string userId, string deviceToken)
    {
        var docRef = _firestoreDb.Collection("userDeviceTokens").Document(userId);
        await docRef.SetAsync(new { tokens = FieldValue.ArrayUnion(deviceToken) }, SetOptions.MergeAll);
    }

    public async Task<List<string>> GetDeviceTokensAsync(string userId)
    {
        var docRef = _firestoreDb.Collection("userTokens").Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            var tokens = snapshot.GetValue<List<string>>("tokens");
            return tokens;
        }
        else
        {
            return new List<string>();
        }
    }
}
