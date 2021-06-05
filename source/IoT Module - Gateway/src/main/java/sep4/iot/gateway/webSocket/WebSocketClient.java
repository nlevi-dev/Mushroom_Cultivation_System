package sep4.iot.gateway.webSocket;

import org.json.JSONException;
import org.json.JSONObject;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.WebSocket;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionStage;

/**
 * WebSocket listener class used for the connection between the
 * Gateway server and Loriot server
 *
 * @author Daria Popa
 * @author Natali Munk-Jakobsen
 * @version 1.0
 * @since 26-05-2021
 */
public class WebSocketClient implements WebSocket.Listener {

    private WebSocket server = null;
    private ArrayList<String> list;

    // Send down-link message to device
    // Must be in Json format according to https://github.com/ihavn/IoT_Semester_project/blob/master/LORA_NETWORK_SERVER.md
    /**
     * Method for sending a downlink message through the Loriot server
     * @param jsonTelegram - the downlink message to be sent
     */
    public void sendDownLink(String jsonTelegram) {
        server.sendText(jsonTelegram,true);
    }

    /**
     * Method used to return one new data String form the list.
     * Acts like a queue - FIFO.
     * @return - the oldest data String from the list
     */
    public String getUpLink(){
        String ret=null ;
        if(list!=null){
            if(list.size()!=0){
                ret = list.get(0);
                list.remove(0);
            }
        }
        return ret;
    }

    // E.g. url: "wss://iotnet.teracom.dk/app?token=???="
    /**
     * Constructor used for setting up and joining the web socket connection to Loriot
     * @param url - the URL for the socket connection
     */
    public WebSocketClient(String url) {
        list = new ArrayList<>();
        HttpClient client = HttpClient.newHttpClient();
        CompletableFuture<WebSocket> ws = client.newWebSocketBuilder()
                .buildAsync(URI.create(url), this);
        server = ws.join();
    }

    //abort connection
    public void abortConnection(){
        server.abort();
    }

    /**
     * Method called when the connection is opened successfully
     * @param webSocket the web socket to which the connection was established
     */
    //onOpen()
    public void onOpen(WebSocket webSocket) {
        // This WebSocket will invoke onText, onBinary, onPing, onPong or onClose methods on the associated listener (i.e. receive methods) up to n more times
        webSocket.request(1);
        System.out.println("WebSocket Listener has been opened for requests.");
    }

    /**
     * Method called if an error occurs in the server side
     * @param webSocket the web socket through which the connection is established
     * @param error the error thrown by the server
     */
    //onError()
    public void onError(WebSocket webSocket, Throwable error) {
        System.out.println("A " + error.getCause() + " exception was thrown.");
        System.out.println("Message: " + error.getLocalizedMessage());
       // webSocket.abort();
    };

    /**
     * Method called when the connection is closed
     * @param webSocket the web socket where the connection was closed
     * @param statusCode the status code for the closing error/message
     * @param reason the reason the connection was closed
     * @return a completion stage which prints out a message in the console at the end
     */
    //onClose()
    public CompletionStage<?> onClose(WebSocket webSocket, int statusCode, String reason) {
        System.out.println("WebSocket closed!");
        System.out.println("Status:" + statusCode + " Reason: " + reason);
        return new CompletableFuture().completedFuture("onClose() completed.").thenAccept(System.out::println);
    };

    /**
     * Method for pinging the server
     * @param webSocket the web socket which holds the connection
     * @param message the pinged message
     * @return a completion stage which prints out a message in the console to indicate method completion
     */
    //onPing()
    public CompletionStage<?> onPing(WebSocket webSocket, ByteBuffer message) {
        webSocket.request(1);
        System.out.println("Ping: Client ---> Server");
        System.out.println(message.asCharBuffer().toString());
        return new CompletableFuture().completedFuture("Ping completed.").thenAccept(System.out::println);
    };

    /**
     * Method for receiving a pong from the server
     * @param webSocket the web socket which holds the connection
     * @param message the received message
     * @return a completion stage which prints out a message in the console to indicate method completion
     */
    //onPong()
    public CompletionStage<?> onPong(WebSocket webSocket, ByteBuffer message) {
        webSocket.request(1);
        System.out.println("Pong: Client ---> Server");
        System.out.println(message.asCharBuffer().toString());
        return new CompletableFuture().completedFuture("Pong completed.").thenAccept(System.out::println);
    };

    /**
     * Method called when new information is heard in the Loriot network server
     * @param webSocket - the web socket connected to
     * @param data - the received data
     * @param last - a boolean representing if the message was the last one received
     * @return a completion stage action i.e printing out a message
     */
    //onText()
    public CompletionStage<?> onText(WebSocket webSocket, CharSequence data, boolean last) {
        String indented = null;
        try {
            indented = (new JSONObject(data.toString()))
                    .toString(4);
            list.add(data.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        webSocket.request(1);
        return new CompletableFuture().completedFuture("onText() completed.").thenAccept(System.out::println);
    };
}
