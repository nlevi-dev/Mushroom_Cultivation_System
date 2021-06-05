package via.sep4.Persistence;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.Base64;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.converter.scalars.ScalarsConverterFactory;

public class WebClient {
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is responsible for implementing the Retrofit library, enabling connections to the webservice.
     */
    public static long date_from = 0;
    public static long date_to = 0;
    
    //URL for connection
    private static final String BASE_URL = "https://mushroompp.nlevi.dev";

    private static final Gson gson = new GsonBuilder()
            .setLenient()
            .create();

    private static final Retrofit.Builder retrofitBuilder = new Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(ScalarsConverterFactory.create())
            .addConverterFactory(GsonConverterFactory.create(gson));

    private static Retrofit retrofit = retrofitBuilder.build();

    private static final OkHttpClient.Builder httpClient
            = new OkHttpClient.Builder();

    private static final HttpLoggingInterceptor logging
            = new HttpLoggingInterceptor()
            .setLevel(HttpLoggingInterceptor.Level.BODY);

    private static String tempJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJrZXkiOiIxIiwibmJmIjoxNjE3ODA5MDk4LCJleHAiOjE4OTM0NTI0MDAsImlhdCI6MTYxNzgwOTA5OH0.YHBQxZOg73Vmm3n6iKH7Ew4rJ9t0Q7VUakFriTsdpig";

    private static SpecimenAPI specimenAPI = createService(SpecimenAPI.class);
    private static UserAPI userAPI = createService(UserAPI.class);
    private static HardwareAPI hardwareAPI = createService(HardwareAPI.class);
    private static StatusAPI statusAPI = createService(StatusAPI.class);
    private static final MiscAPI miscAPI = createService(MiscAPI.class);

    public static <S> S createService(Class<S> serviceClass) {
        if (!httpClient.interceptors().contains(logging)) {
            httpClient.addInterceptor(logging);
            retrofitBuilder.client(httpClient.build());
            retrofit = retrofitBuilder.build();
        }
        return retrofit.create(serviceClass);
    }

    public static <S> S createService(Class<S> serviceClass, final String token) {
        httpClient.interceptors().clear();
        if (token != null) {
            httpClient.addInterceptor( chain -> {
                Request original = chain.request();
                Request.Builder builder1 = original.newBuilder()
                        .header("Authorization", "Bearer " + token);
                Request request = builder1.build();
                return chain.proceed(request);
            });
            httpClient.addInterceptor(logging);
            retrofitBuilder.client(httpClient.build());
            retrofit = retrofitBuilder.build();
        }
        return retrofit.create(serviceClass);
    }

    public static SpecimenAPI getSpecimenAPI()
    {
        return specimenAPI;
    }
    public static UserAPI getUserAPI() {
        return userAPI;
    }
    public static HardwareAPI getHardwareAPI() {
        return hardwareAPI;
    }
    public static StatusAPI getStatusAPI() {
        return statusAPI;
    }
    public static MiscAPI getMiscAPI() {
        return miscAPI;
    }
    public static void token(String auth, final Callback<String> boolCallback)
    {
        String s = "Basic " + Base64.getEncoder().encodeToString(auth.getBytes());
        Call<String> tokenCall = getMiscAPI().getToken(s);
        tokenCall.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                if(response.body() != null) {
                    tempJWT = response.body();
                    specimenAPI = createService(SpecimenAPI.class, tempJWT);
                    userAPI = createService(UserAPI.class, tempJWT);
                    hardwareAPI = createService(HardwareAPI.class, tempJWT);
                    statusAPI = createService(StatusAPI.class, tempJWT);
                    boolCallback.onResponse(call, response);
                    //callback chained to divert async to MutableLiveData, where we can use postValue,
                    //so we don't have problems with returning values from async
                }
            }
            @Override
            public void onFailure(Call<String> call, Throwable t) {
                System.out.println("AAAAA");
            }
        });
    }
}