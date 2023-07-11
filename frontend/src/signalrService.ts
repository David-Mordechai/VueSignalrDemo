import { HubConnectionBuilder, LogLevel, HubConnection, HubConnectionState } from '@microsoft/signalr';
export default class SignalrService {

    ReceiveReport: ((color: string, command: string) => void) | undefined;
    
    private connection: HubConnection;

    constructor(url: string) {
        this.connection = new HubConnectionBuilder()
            .withUrl(url, {
                withCredentials: false
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        this.connection.on("receiveReport", (color, command) => {
            console.log(color);
            this.ReceiveReport?.call(this, color, command);
        });
    }

    start() : SignalrService {
        this.connection.start().catch((err) => console.log(err));
        return this;
    }

    stop() {
        if (this.connection.state !== HubConnectionState.Disconnected)
            this.connection.stop();
    }

    SendCommand(commandType: string) {
        console.log(commandType);
        this.connection.send('sendCommand', commandType);
    }
}