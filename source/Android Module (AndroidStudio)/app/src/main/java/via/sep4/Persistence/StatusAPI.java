package via.sep4.Persistence;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;
import via.sep4.Model.Status;

public interface StatusAPI {
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("status/key/{status_key}")
    Call<Status> getStatusByKey(@Path("status_key") int status_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @DELETE("status/key/{status_key}")
    Call<Status> deleteStatusByKey(@Path("status_key") int status_key);

    @PUT("status/key/{status_key}")
    Call<Status> updateStatus(@Body Status status, @Path("status_key") int status_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @POST("specimen/key/{specimen_key}/status")
    Call<Status> createStatus(@Path("specimen_key") int specimen_key, @Body Status status);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("specimen/key/{specimen_key}/status")
    Call<List<Status>> getSpecimenStatusBySpecimenKey(@Path("specimen_key") int specimen_key);

}
