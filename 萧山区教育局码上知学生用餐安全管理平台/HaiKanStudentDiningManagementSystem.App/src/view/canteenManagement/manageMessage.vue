<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.cuisine.query.totalCount"
        :pageSize="stores.cuisine.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.cuisine.query.kw1"
                      placeholder="请输入培训主题"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'batch'"
                    v-show="schoolshow1==1"
                    class="txt-danger"
                    icon="md-trash"
                    title="批量删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button  v-can="'refresh'" icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                   v-show="schoolshow1==1"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
                <Button
                  v-can="'import'"
                       v-show="schoolshow1==1"             
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                >导入</Button>
                <Button
                  v-can="'export'"
                   v-show="schoolshow1==1"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                >导出</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.cuisine.data"
          :columns="stores.cuisine.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true"  v-show="schoolshow1==1">
                <Button type="error" v-can="'delete'" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true"  v-show="schoolshow1==1">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="培训主题" prop="theme">
            <Input v-model="formModel.fields.theme" placeholder="请输入培训主题"  type="textarea" :readonly="this.formModel.mode === 'show'" />
          </FormItem>
        </Row>
          <Row :gutter="16">
            <FormItem label="培训日期" >
            <DatePicker v-if="this.formModel.mode != 'show'" type="date" :value="formModel.fields.trainTime" @on-change="formModel.fields.trainTime=$event" format="yyyy-MM-dd" placeholder="选择时间" style="width: 384px;"></DatePicker>
            <Input v-if="this.formModel.mode === 'show'" v-model="formModel.fields.trainTime" :readonly="this.formModel.mode === 'show'" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="培训对象" prop="trainName">
            <Input :readonly="this.formModel.mode === 'show'" v-model="formModel.fields.trainName" placeholder="培训人员名称"/>
          </FormItem>
        </Row>
         <Row :gutter="16">
          <FormItem label="培训地址">
            <Input :readonly="this.formModel.mode === 'show'" v-model="formModel.fields.address" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="培训课时">
            <Input :readonly="this.formModel.mode === 'show'" v-model="formModel.fields.trainLession"  />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="上午培训人数">
            <Input :readonly="this.formModel.mode === 'show'" v-model="formModel.fields.mnum" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="下午培训人数">
            <Input :readonly="this.formModel.mode === 'show'"  v-model="formModel.fields.anum" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="授课人" prop="speaker">
            <Input :readonly="this.formModel.mode === 'show'" v-model="formModel.fields.speaker" placeholder="授课人" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <quill-editor
            @focus="onEditorFocus($event)"
            class="mec"
            v-model="formModel.fields.content"
          ></quill-editor>
          <!-- <FormItem label="主要培训内容" prop="content">
            <Input :autosize="{minRows: 2,maxRows: 10}" v-model="formModel.fields.content" placeholder="主要培训内容"  type="textarea"/>
          </FormItem> -->
        </Row>
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 150px;">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formdispatch" label-position="left">
        
        <Row :gutter="16">
          <FormItem label="培训主题">
            <Input v-model="formModel1.fields.theme" :readonly="true" type="textarea"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="培训日期">
            <Input v-model="formModel1.fields.trainTime" :readonly="true" />
          </FormItem>
        </Row>      
        <Row :gutter="16">
          <FormItem label="培训对象">
            <Input v-model="formModel1.fields.trainName" :readonly="true" />
          </FormItem>
        </Row>
         <Row :gutter="16">
          <FormItem label="培训地址">
            <Input v-model="formModel1.fields.address" :readonly="true" />
          </FormItem>
        </Row>
         <Row :gutter="16">
          <FormItem label="培训课时">
            <Input v-model="formModel1.fields.trainLession" :readonly="true" />
          </FormItem>
        </Row>
         <Row :gutter="16">
          <FormItem label="上午培训人数">
            <Input v-model="formModel1.fields.mnum" :readonly="true" />
          </FormItem>
        </Row>
         <Row :gutter="16">
          <FormItem label="下午培训人数">
            <Input v-model="formModel1.fields.anum" :readonly="true" />
          </FormItem>
        </Row>
       <Row :gutter="16">
          <FormItem label="授课人">
            <Input v-model="formModel1.fields.speaker" :readonly="true" />
          </FormItem>
        </Row>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="主要培训内容" prop="content">
            <Input :autosize="true" v-model="formModel1.fields.content" :readonly="true" type="textarea"/>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="信息导入"
      v-model="formimport.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          v-can="'model'"
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="信息导入模板下载"
        >信息导入模板下载</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
 MessageList,
 MessageGet,
 deleteDepartment,
 batchCommand,
 Import,
 CuisineCreate,
 CuisineEdit,
Export
} from "@/api/message/message";
import config from "@/config";
export default {
  name: "manageMessage_page",
  components: {
    DzTable
  },
  data() {
    return {
      //导入
      url: config.baseUrl.dev,
      importdisable: false,
      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      schoolshow1:0,
      formimport: {
        opened: false
      },
      schoolshow1:0,
      //附件上传
      imgshow1: false,
      img1: "",
      img: [],
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" }
      },
      inglist:[],
      model1: [],
      model2: [],
      stores: {
        cuisine: {
          query: {
            totalCount: 0,
             //d定义11
              schoolguid:"",
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "trainUuid" },
            { title: "培训主题", key: "theme" , sortable: true},
            { title: "培训时间", key: "trainTime" },
            { title: "主讲人", key: "speaker" },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          trainUuid: "",
          theme:"",
          schoolUuid:"",
          trainTime:"", 
          anum:"",
          mnum:"",
          trainLession:"",
          address:"",      
          burdening: "",
          ingredient: "",
          abstracts: "",
          content:"",
          trainName:"",
          isDeleted: 0,
          speaker: ""
        },
        type: "荤菜",
        cuisinetype: [
          { type: "荤菜" },
          { type: "半荤菜" },
          { type: "素菜" },
          { type: "甜点" },
          { type: "其它" }
        ],
        rules: {
          // cuisineName: [
          //   { type: "string", required: true, message: "请输入菜品名称" }
          // ]
        }
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
          trainUuid: "",
          theme:"",
          schoolUuid:"",
          trainTime:"",
          anum:"",
          mnum:"",
          trainLession:"",
          address:"",       
          burdening: "",
          ingredient: "",
          abstracts: "",
          content:"",
          trainName:"",
          isDeleted: 0,
          speaker: ""
        }
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel.mode === "show") {
        return "详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.trainUuid);
    } //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
       if (this.$store.state.user.schoolguid == null) {
                this.schoolshow1=0;
                
      }else{
         this.schoolshow1=1;
      }
      //赋值222
        this.stores.cuisine.schoolguid = this.$store.state.user.schoolguid;
      MessageList(this.stores.cuisine.query).then(res => {
        console.log(res);
        this.stores.cuisine.data = res.data.data;
        this.stores.cuisine.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.cuisine.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.cuisine.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.formModel.fields.content = "";
      this.formModel.fields.theme = "";
      this.formModel.fields.speaker = "";
      this.formModel.fields.schoolUuid = "";
      this.formModel.fields.abstracts = "";
      this.formModel.fields.accessory = "";
      this.formModel.fields.trainName = "";
      this.formModel.fields.trainLession = "";
      this.formModel.fields.address = "";
      this.formModel.fields.anum = "";
      this.formModel.fields.mnum = "";
      this.formModel.fields.trainTime="";
      this.img1 = "";
      this.imgcopy1 = "";
      this.img=[];
      this.imgshow1 = false;
      this.model1=[];
      this.model2=[];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.trainUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //类别选择
    changetype(e) {
      this.formModel.fields.cuisineType = e;
    },
        //食材
    doIngredientGet(guid){
      MessageGet(guid).then(res=>{
        this.formModel.fields = res.data.data;
        this.formModel1.fields = res.data.data;
        console.log(this.formModel.fields)
      //this.inglist = res.data.data;
      })
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.mode = "create";        
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
     
    },
    //右边编辑
    handleEdit(row) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      //this.doLoadData(row.trainUuid);
      this.doIngredientGet(row.trainUuid);

    },
    //右边详情
    handleDetail(row) {
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
       this.doIngredientGet(row.trainUuid);
             
      // this.doLoadData(row.trainUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      CuisineGet(id).then(res => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.formModel1.fields = res.data.data[0];
          if (res.data.data.accessory != null) {
            let imgage = res.data.data.accessory.split("|");
            if (imgage[0] != "") {
              for (let k = 0; k < imgage.length; k++) {
                this.img[k] = this.url + res.data.data.accessory.split("|")[k];
              }
              this.img1 = this.url + res.data.data.accessory.split("|")[0];
              this.imgcopy1 = res.data.data.accessory;
              this.imgshow1 = true;
            } else {
              this.img1 = "";
              this.imgcopy1 = "";
              this.imgshow1 = false;
            }
          }
          this.model1=res.data.data.ingredient.split(",");
          this.model2=res.data.data.burdening.split(",");
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      if (this.formModel.fields.theme=="") {
        this.$Message.warning("请填写培训主题!");
        return;
      }
      if (this.formModel.fields.trainName=="") {
        this.$Message.warning("请填写培训人员名称!");
        return;
      }
      if (this.formModel.fields.speaker=="") {
        this.$Message.warning("请填写授课人");
        return;
      }
      if (this.formModel.fields.address=="") {
        this.$Message.warning("请填写地址");
        return;
      }
       let reg1 = /^[0-9]*$/;
      if (!reg1.test(this.formModel.fields.anum)) {
        this.$Message.warning("上午培训人数格式不合法!");
        return;
      }
       let reg2 = /^[0-9]*$/;
      if (!reg2.test(this.formModel.fields.mnum)) {
        this.$Message.warning("下午培训人数格式不合法!");
        return;
      }
      if (this.formModel.fields.content=="") {
        this.$Message.warning("请填写主要培训内容!");
        return;
      }
       if (this.formModel.mode === "create") {
        this.docreateDispatch();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
    //添加（保存）
    docreateDispatch() {
      this.formModel.fields.schoolUuid=this.$store.state.user.schoolguid;
      CuisineCreate(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
       //this.$refs["formdispatch"].resetFields();
    },
    //编辑（保存）
    doEditDispatch() { 
      CuisineEdit(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //导入相关操作
    handleImportCuisine() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    //下载模板
    handleimportmodel() {
      window.location.href =
        this.url + "UploadFiles/ImportModel/培训.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
    },
    async handleimport() {
      this.importdisable = true; 
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        await Import(this.exceldata).then(res => {
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
    //导出
    handleExportCuisine(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doCuisineExport(command);
        }
      });
    },
    doCuisineExport(command) {
      console.log(this.selectedRowsId.join(","));
      Export(this.selectedRowsId.join(",")).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          //this.loadDispatchList();
          this.formModel.selection = [];
          console.log(res);
          window.location.href = config.baseUrl.dev + res.data.data;
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    onEditorFocus(event) {
      if (this.formModel.mode === "show") {
        event.enable(false);
      } else {
        event.enable(true);
      }
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  }
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}
.mec {
  height: 300px;
}
</style>