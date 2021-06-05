package via.sep4.Persistence;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;
import retrofit2.http.Query;
import via.sep4.Model.SensorData;
import via.sep4.Model.SensorDataList;
import via.sep4.Model.Specimen;

public interface SpecimenAPI {
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This interface is the Retrofit API interface for specimens.
     */
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @POST("specimen")
    Call<Specimen> createSpecimen(@Body Specimen specimen);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("specimen")
    Call<List<Specimen>> getSpecimens();

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("specimen/key/{specimen_key}")
    Call<Specimen> getSpecimen(@Path("specimen_key") int specimen_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @DELETE("specimen/key/{specimen_key}")
    Call<Specimen> deleteSpecimenByKey(@Path("specimen_key") int specimen_key);

    @PUT("specimen/key/{specimen_key}")
    Call<Specimen> updateSpecimen(@Body Specimen specimen,@Path("specimen_key") int specimen_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("specimen/key/{specimen_key}/sensor")
    Call<ArrayList<SensorData>> getSpecimenSensor(@Path("specimen_key") int specimen_key, @Query("datetime_from") long datetime_from, @Query("datetime_until") long datetime_until);

}