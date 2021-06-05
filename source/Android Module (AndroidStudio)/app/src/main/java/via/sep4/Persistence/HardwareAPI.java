package via.sep4.Persistence;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;
import via.sep4.Model.Hardware;
import via.sep4.Model.SensorData;

public interface HardwareAPI {
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @POST("hardware")
    Call<Hardware> createHardware(@Body Hardware hardware);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("hardware")
    Call<List<Hardware>> getHardware();

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("hardware/key/{hardware_key}")
    Call<Hardware> getHardwareByKey(@Path("hardware_key") int hardware_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @DELETE("hardware/key/{hardware_key}")
    Call<Hardware> deleteHardwareByKey(@Path("hardware_key") int hardware_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @PATCH("hardware/key/{hardware_key}")
    Call<Hardware> setHardwareID(@Path("hardware_key") int hardware_key,@Field("hardware_id") String hardware_id);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("hardware/id/{hardware_id}/sensor")
    Call<SensorData> getHardwareSensorData(@Path("hardware_id") int hardware_id);
}
