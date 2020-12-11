<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.demo.query.totalCount"
        :pageSize="stores.demo.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="23">
              <Col span="23" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
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
          :data="stores.demo.data"
          :columns="stores.demo.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" v-can="'delete'" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
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
          <FormItem label="留样人员" prop="sampleName">
            <Input
              v-model="formModel.fields.sampleName"
              :readonly="checkShow()"
              placeholder="请输入留样人员"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="留样时间" prop="sampledAt">
            <Date-picker
              v-model="formModel.fields.sampledAt"
              @on-change="formModel.fields.sampledAt=$event"
              type="datetime"
              placeholder="请选择留样时间"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="审核人员" prop="auditorName">
            <Input
              v-model="formModel.fields.auditorName"
              :readonly="checkShow()"
              placeholder="请输入审核人员"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="留样餐次" prop="mealTime">
            <Select
              filterable
              v-model="formModel.fields.mealTime"
              style="width:100%"
              placeholder="请选择留样餐次"
              :disabled="checkShow()"
              filterable
              :label-in-value="true"
              @on-change="Getfood"
            >
              <Option
                v-for="item in stores.demo.sources.mealTimeNameList"
                :value="item.typeId"
                :key="item.typeId"
              >{{ item.description }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="留样数量/g" prop="weight">
            <Input-number
              v-model="formModel.fields.weight"
              :min="0"
              :readonly="checkShow()"
              placeholder="请输入留样数量"
              style="width:100%;"
            ></Input-number>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="留样规定/h" prop="hours">
            <Input-number
              v-model="formModel.fields.hours"
              :min="0"
              :readonly="checkShow()"
              placeholder="请输入留样数量"
              style="width:100%;"
            ></Input-number>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="留样食品" prop="foodName">
            <!-- <Input
              v-model="formModel.fields.foodName"
              :readonly="checkShow()"
              placeholder="请输入留样食品"
            />-->
            <Select
              v-model="model1"
              multiple
              style="width: 100%;"
              :disabled="checkShow()"
              filterable
              :label-in-value="true"
              @on-change="GetfoodName"
            >
              <Option
                v-for="item in foodList"
                :key="item.goodsId"
                :value="item.goodsId"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="到期时间" prop="maturedAt">
            <Date-picker
              v-model="formModel.fields.maturedAt"
              @on-change="formModel.fields.maturedAt=$event"
              type="datetime"
              placeholder="请选择到期时间"
              style="width: 100%"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="消样时间" prop="eliminatedAt">
            <Date-picker
              v-model="formModel.fields.eliminatedAt"
              @on-change="formModel.fields.eliminatedAt=$event"
              type="datetime"
              placeholder="请选择消样时间"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="note">
            <Input
              type="textarea"
              v-model="formModel.fields.note"
              :readonly="checkShow()"
              placeholder="请输入备注"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left;"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  type="ios-trash-outline"
                  style="float: right;"
                  @click.native="handleRemove(item.name)"
                   v-show="!checkShow()"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{scene:'留样图片',groupType:'reservedSample'}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          v-if="!checkShow()"
        >保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  GetList, //显示列表
  GetCreate, //新增
  GetShow, //获取选定信息
  batchCommand, //批量删除
  deleteDepartment, //单个删除
  GetEdit, //编辑
  GetfoodList,
  GetTypeList
} from "@/api/PurchasesInfo/samples";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "samples",
  components: {
    DzTable,
  },
  data() {
    return {
      options3: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
      },
      
      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,

      foodList: [],
      model1: [],
      stores: {
        demo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            mealTimeNameList:[],
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "id" },
            { title: "留样时间", key: "sampledAt", sortable: true, ellipsis: true },
            { title: "留样人员", key: "sampleName", sortable: true, ellipsis: true },
            { title: "审核人员", key: "auditorName", sortable: true, ellipsis: true },
            { title: "留样餐次", key: "mealTimeName", sortable: true, ellipsis: true },
            { title: "留样数量", key: "weight", sortable: true, ellipsis: true },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          foodIds: "",
          foodName: "",
          note: "",
          img: "",
          weight: 0,
          hours: 0,
          mealTime: "",
          mealTimeName: "",
          eliminateId: 0,
          eliminateName: "",
          auditorId: 0,
          auditorName: "",
          sampleName: "",
          sampledAt: "",
          maturedAt: "",
          eliminatedAt: "",
          note: "",
        },
        rules: {
          sampleName: [
            { type: "string", required: true, message: "留样人员不能为空" },
          ],
        },
      },
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
        return "信息详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.id);
    }, //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      GetList(this.stores.demo.query).then((res) => {
        console.log(res);
        this.stores.demo.data = res.data.data;
        this.stores.demo.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.demo.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.demo.query.pageSize = pageSize;
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
    checkShow() {
      return this.formModel.mode === "show";
    },
    doGetTypeList(){
      this.doGetfoodList();
      GetTypeList().then(res=>{
        this.stores.demo.sources.mealTimeNameList=res.data.data;
      });
    },
    doGetfoodList() {
      GetfoodList(this.formModel.fields.mealTimeName).then((res) => {
        this.foodList = res.data.data;
      });
    },
    Getfood(e) {
      if(e!=undefined){
        if(e.label!=this.formModel.fields.mealTimeName){
          this.model1=[];
          this.formModel.fields.foodIds="";
          this.formModel.fields.foodName="";
        }
        this.formModel.fields.mealTimeName = e.label;
      }
      this.doGetfoodList();
    },
    //详情显示
    handleDetail(e) {
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(e.id);
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
      this.doGetTypeList();
    },
    //右边编辑
    handleEdit(row) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(row.id);
    },
    //查询当前行信息
    doLoadData(id) {
      GetShow(id).then((res) => {
        if (res.data.code === 200) {
          this.model1 = [];
          this.formModel.fields = res.data.data;
          if (res.data.data.foodIds != null) {
            // this.model1 = res.data.data.foodIds.split(",");
            if (res.data.data.foodIds.split(",").length > 0) {
              for (
                let k = 0;
                k < res.data.data.foodIds.split(",").length;
                k++
              ) {
                this.model1.push(parseInt(res.data.data.foodIds.split(",")[k]));
              }
            }
            if(isNaN(this.model1[this.model1.length-1])){
              this.model1.pop();
            }
          }
          if (res.data.data.img != null) {
            let list = res.data.data.img.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  list[i],
                status: "finished",
                name: list[i],
                fileName: list[i]
              });
            }
          }
          this.doGetTypeList();
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    GetfoodName(e) {
      let data = [];
      for (let k = 0; k < e.length; k++) {
        data.push(e[k].label);
      }
      this.formModel.fields.foodName = data.join(",");
      this.formModel.fields.foodIds = this.model1.join(",") + ",";
    },
    //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateUserForm();
      if (valid) {
        // let reg = /^([\u4e00-\u9fa5]){2,7}$/;
        // if (!reg.test(this.formModel.fields.organName)) {
        //   this.$Message.warning("姓名不合法!");
        //   return;
        // }else{
        //     this.$Message.warning("姓名不能为空!");
        //     return;
        // }
        if (this.formModel.mode === "create") {
          this.docreateDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditDispatch();
        }
      }
    },
    //添加（保存）
    docreateDispatch() {
      if(this.formModel.fields.foodName==""){
        this.formModel.fields.foodIds="";
      }
      GetCreate(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      if (
        this.formModel.fields.sampledAt != "" &&
        this.formModel.fields.sampledAt != null
      ) {
        this.formModel.fields.sampledAt = new Date(
          Date.parse(new Date(this.formModel.fields.sampledAt)) +
            8 * 3600 * 1000
        );
      }
      if (
        this.formModel.fields.maturedAt != "" &&
        this.formModel.fields.maturedAt != null
      ) {
        this.formModel.fields.maturedAt = new Date(
          Date.parse(new Date(this.formModel.fields.maturedAt)) +
            8 * 3600 * 1000
        );
      }
      if (
        this.formModel.fields.eliminatedAt != "" &&
        this.formModel.fields.eliminatedAt != null
      ) {
        this.formModel.fields.eliminatedAt = new Date(
          Date.parse(new Date(this.formModel.fields.eliminatedAt)) +
            8 * 3600 * 1000
        );
      }
      GetEdit(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.model1 = [];
      this.formModel.fields.img="";
      this.defaultList=[];
      this.uploadList=[];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.id);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then((res) => {
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
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.img = this.uploadList
            .map(x => x.fileName)
            .join(",");
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    toUpResult() {
      console.log(1111);
      console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields.img);
        if (this.formModel.fields.img != null) {
          if (this.formModel.fields.img.length > 0) {
            this.formModel.fields.img += ",";
          }
          this.formModel.fields.img += response.data;
        } else {
          this.formModel.fields.img = response.data;
        }
        await this.uploadList.push({
          url: response.data.replace("\\", "/"),
          status: "finished",
          name: response.data,
          fileName: response.data
        });
      } else {
        this.$Message.warning(response.message);
      }
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/UpImage/ToUPPhoto/UpPhoto";
  },
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

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>